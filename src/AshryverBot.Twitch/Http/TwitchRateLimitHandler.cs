using System.Collections.Concurrent;
using System.Globalization;
using System.Net;
using Microsoft.Extensions.Logging;

namespace AshryverBot.Twitch.Http;

/// <summary>
/// Honors Twitch Helix rate-limit headers (<c>Ratelimit-Limit</c>, <c>Ratelimit-Remaining</c>,
/// <c>Ratelimit-Reset</c>) and retries with backoff on HTTP 429. One bucket per access token,
/// matching Twitch's per-user/per-app-token bucketing.
/// See https://dev.twitch.tv/docs/api/guide/#twitch-rate-limits.
/// </summary>
public sealed class TwitchRateLimitHandler : DelegatingHandler
{
    internal const string LimitHeader = "Ratelimit-Limit";
    internal const string RemainingHeader = "Ratelimit-Remaining";
    internal const string ResetHeader = "Ratelimit-Reset";

    private const int DefaultMaxRetries = 2;
    private static readonly TimeSpan _defaultFallbackBackoff = TimeSpan.FromSeconds(1);

    private readonly ConcurrentDictionary<string, Bucket> _buckets = new(StringComparer.Ordinal);
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<TwitchRateLimitHandler> _logger;
    private readonly int _maxRetries;

    public TwitchRateLimitHandler(TimeProvider timeProvider, ILogger<TwitchRateLimitHandler> logger)
        : this(timeProvider, logger, DefaultMaxRetries)
    {
    }

    internal TwitchRateLimitHandler(TimeProvider timeProvider, ILogger<TwitchRateLimitHandler> logger, int maxRetries)
    {
        _timeProvider = timeProvider;
        _logger = logger;
        _maxRetries = maxRetries;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var bucket = _buckets.GetOrAdd(GetBucketKey(request), static _ => new Bucket());

        var attempt = 0;

        while (true)
        {
            var wait = bucket.Reserve(_timeProvider);

            if (wait > TimeSpan.Zero)
            {
                _logger.LogDebug("Twitch rate limit reached; waiting {Delay} before sending {Method} {Uri}.",
                    wait, request.Method, request.RequestUri);
                await Task.Delay(wait, _timeProvider, cancellationToken);
            }

            var response = await base.SendAsync(request, cancellationToken);
            bucket.UpdateFromResponse(response);

            if (response.StatusCode != HttpStatusCode.TooManyRequests || attempt >= _maxRetries)
                return response;

            var retryAfter = GetRetryAfter(response, _timeProvider) ?? _defaultFallbackBackoff;
            _logger.LogWarning("Twitch returned 429 for {Method} {Uri}; retrying in {Delay} (attempt {Attempt}/{MaxRetries}).",
                request.Method, request.RequestUri, retryAfter, attempt + 1, _maxRetries);

            response.Dispose();
            await Task.Delay(retryAfter, _timeProvider, cancellationToken);
            attempt++;
        }
    }

    private static string GetBucketKey(HttpRequestMessage request)
        => request.Headers.Authorization?.Parameter ?? string.Empty;

    private static TimeSpan? GetRetryAfter(HttpResponseMessage response, TimeProvider timeProvider)
    {
        var retryAfter = response.Headers.RetryAfter;
        if (retryAfter is not null)
        {
            if (retryAfter.Delta is { } delta && delta > TimeSpan.Zero)
                return delta;
            if (retryAfter.Date is { } date)
            {
                var diff = date - timeProvider.GetUtcNow();
                if (diff > TimeSpan.Zero) return diff;
            }
        }

        if (TryReadUnixSeconds(response, ResetHeader, out var resetAt))
        {
            var diff = resetAt - timeProvider.GetUtcNow();
            if (diff > TimeSpan.Zero) return diff;
        }

        return null;
    }

    private static bool TryReadUnixSeconds(HttpResponseMessage response, string headerName, out DateTimeOffset value)
    {
        value = default;
        if (!response.Headers.TryGetValues(headerName, out var values)) return false;

        var raw = values.FirstOrDefault();
        if (!long.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var seconds))
            return false;

        value = DateTimeOffset.FromUnixTimeSeconds(seconds);
        return true;
    }

    private static bool TryReadInt(HttpResponseMessage response, string headerName, out int value)
    {
        value = 0;
        if (!response.Headers.TryGetValues(headerName, out var values)) return false;

        var raw = values.FirstOrDefault();
        return int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
    }

    private sealed class Bucket
    {
        private readonly Lock _lock = new();
        private int _remaining = int.MaxValue;
        private DateTimeOffset _resetAt = DateTimeOffset.MinValue;

        public TimeSpan Reserve(TimeProvider timeProvider)
        {
            lock (_lock)
            {
                var now = timeProvider.GetUtcNow();

                if (_remaining <= 0 && _resetAt > now)
                    return _resetAt - now;

                if (_remaining != int.MaxValue)
                    _remaining--;

                return TimeSpan.Zero;
            }
        }

        public void UpdateFromResponse(HttpResponseMessage response)
        {
            lock (_lock)
            {
                if (TryReadInt(response, RemainingHeader, out var remaining))
                    _remaining = remaining;
                if (TryReadUnixSeconds(response, ResetHeader, out var resetAt))
                    _resetAt = resetAt;
            }
        }
    }
}
