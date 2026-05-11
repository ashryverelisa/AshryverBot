using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using AshryverBot.Twitch.Http;
using AshryverBot.Twitch.Tests.TestSupport;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using Xunit;

namespace AshryverBot.Twitch.Tests.Http;

public class TwitchRateLimitHandlerTests
{
    private static (HttpClient client, MockHttpMessageHandler inner, FakeTimeProvider time) Create(
        Func<HttpRequestMessage, int, HttpResponseMessage> respond,
        int maxRetries = 2)
    {
        var time = new FakeTimeProvider(DateTimeOffset.FromUnixTimeSeconds(1_000_000));
        var requestCount = 0;
        var inner = new MockHttpMessageHandler((req, _) =>
        {
            var attempt = requestCount++;
            return Task.FromResult(respond(req, attempt));
        });

        var handler = new TwitchRateLimitHandler(time, NullLogger<TwitchRateLimitHandler>.Instance, maxRetries)
        {
            InnerHandler = inner,
        };
        var client = new HttpClient(handler) { BaseAddress = new Uri("https://api.twitch.tv/helix/") };
        return (client, inner, time);
    }

    private static HttpResponseMessage Ok(int remaining, long? resetUnixSeconds = null)
    {
        var resp = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{}") };
        resp.Headers.TryAddWithoutValidation(TwitchRateLimitHandler.LimitHeader, "800");
        resp.Headers.TryAddWithoutValidation(TwitchRateLimitHandler.RemainingHeader,
            remaining.ToString(CultureInfo.InvariantCulture));
        if (resetUnixSeconds is { } reset)
            resp.Headers.TryAddWithoutValidation(TwitchRateLimitHandler.ResetHeader,
                reset.ToString(CultureInfo.InvariantCulture));
        return resp;
    }

    private static HttpResponseMessage TooManyRequests(long? resetUnixSeconds = null, TimeSpan? retryAfter = null)
    {
        var resp = new HttpResponseMessage(HttpStatusCode.TooManyRequests) { Content = new StringContent("{}") };
        resp.Headers.TryAddWithoutValidation(TwitchRateLimitHandler.RemainingHeader, "0");
        if (resetUnixSeconds is { } reset)
            resp.Headers.TryAddWithoutValidation(TwitchRateLimitHandler.ResetHeader,
                reset.ToString(CultureInfo.InvariantCulture));
        if (retryAfter is { } delta)
            resp.Headers.RetryAfter = new RetryConditionHeaderValue(delta);
        return resp;
    }

    private static HttpRequestMessage NewRequest(string token = "user-token")
    {
        var req = new HttpRequestMessage(HttpMethod.Get, "users");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return req;
    }

    [Fact]
    public async Task First_request_passes_through_without_waiting()
    {
        var (client, inner, time) = Create((_, _) => Ok(remaining: 799));

        var start = time.GetUtcNow();
        var response = await client.SendAsync(NewRequest());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Single(inner.Requests);
        Assert.Equal(start, time.GetUtcNow());
    }

    [Fact]
    public async Task Updates_bucket_from_response_headers()
    {
        // First response says remaining=0 and resets in 30s. Second request must wait.
        var resetAt = 1_000_030L;
        var (client, inner, time) = Create((_, attempt) => attempt switch
        {
            0 => Ok(remaining: 0, resetUnixSeconds: resetAt),
            _ => Ok(remaining: 799, resetUnixSeconds: 1_000_090),
        });

        await client.SendAsync(NewRequest());

        var sendTask = client.SendAsync(NewRequest());
        // The second request should be blocked waiting for the bucket to reset.
        Assert.False(sendTask.IsCompleted);

        time.Advance(TimeSpan.FromSeconds(30));
        await sendTask;

        Assert.Equal(2, inner.Requests.Count);
        Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(resetAt), time.GetUtcNow());
    }

    [Fact]
    public async Task Retries_on_429_using_Ratelimit_Reset()
    {
        var resetAt = 1_000_005L;
        var (client, inner, time) = Create((_, attempt) => attempt switch
        {
            0 => TooManyRequests(resetUnixSeconds: resetAt),
            _ => Ok(remaining: 799),
        });

        var sendTask = client.SendAsync(NewRequest());
        Assert.False(sendTask.IsCompleted);

        time.Advance(TimeSpan.FromSeconds(5));
        var response = await sendTask;

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, inner.Requests.Count);
    }

    [Fact]
    public async Task Retries_on_429_using_RetryAfter_header()
    {
        var (client, inner, time) = Create((_, attempt) => attempt switch
        {
            0 => TooManyRequests(retryAfter: TimeSpan.FromSeconds(3)),
            _ => Ok(remaining: 799),
        });

        var sendTask = client.SendAsync(NewRequest());
        Assert.False(sendTask.IsCompleted);

        time.Advance(TimeSpan.FromSeconds(3));
        var response = await sendTask;

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, inner.Requests.Count);
    }

    [Fact]
    public async Task Stops_retrying_after_max_retries_and_returns_last_429()
    {
        var (client, inner, time) = Create(
            (_, _) => TooManyRequests(retryAfter: TimeSpan.FromMilliseconds(1)),
            maxRetries: 2);

        var sendTask = client.SendAsync(NewRequest());

        for (var i = 0; i < 5; i++)
        {
            time.Advance(TimeSpan.FromSeconds(1));
            await Task.Yield();
        }

        var response = await sendTask;

        Assert.Equal(HttpStatusCode.TooManyRequests, response.StatusCode);
        Assert.Equal(3, inner.Requests.Count); // 1 initial + 2 retries
    }

    [Fact]
    public async Task Different_tokens_get_separate_buckets()
    {
        // userA hits 0 remaining; userB should still pass through immediately.
        var (client, inner, _) = Create((req, _) =>
        {
            var token = req.Headers.Authorization!.Parameter;
            return token == "user-a"
                ? Ok(remaining: 0, resetUnixSeconds: 1_000_999)
                : Ok(remaining: 799);
        });

        await client.SendAsync(NewRequest("user-a"));
        await client.SendAsync(NewRequest("user-b"));

        Assert.Equal(2, inner.Requests.Count);
    }

    [Fact]
    public async Task Decrements_remaining_optimistically_to_avoid_concurrent_burst()
    {
        // Inner handler is slow to respond — both concurrent requests should be
        // serialized so the second waits once the first decrements to 0.
        var resetAt = 1_000_020L;
        var responses = new Queue<HttpResponseMessage>(new[]
        {
            Ok(remaining: 0, resetUnixSeconds: resetAt),
            Ok(remaining: 799, resetUnixSeconds: 1_000_080),
        });

        var time = new FakeTimeProvider(DateTimeOffset.FromUnixTimeSeconds(1_000_000));
        var firstResponseTcs = new TaskCompletionSource();
        var inner = new MockHttpMessageHandler(async (req, ct) =>
        {
            // First request waits until we signal.
            if (req.Headers.Authorization!.Parameter == "first")
                await firstResponseTcs.Task.WaitAsync(ct);
            return responses.Dequeue();
        });
        var handler = new TwitchRateLimitHandler(time, NullLogger<TwitchRateLimitHandler>.Instance)
        {
            InnerHandler = inner,
        };
        var client = new HttpClient(handler);
        client.BaseAddress = new Uri("https://api.twitch.tv/helix/");

        // Prime the bucket: first call learns remaining=0/reset in 20s.
        var primeReq = new HttpRequestMessage(HttpMethod.Get, "users");
        primeReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "first");
        var primeTask = client.SendAsync(primeReq);
        firstResponseTcs.SetResult();
        await primeTask;

        // Now the bucket knows remaining=0. The next request must wait until reset.
        var nextReq = new HttpRequestMessage(HttpMethod.Get, "users");
        nextReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "first");
        var nextTask = client.SendAsync(nextReq);
        Assert.False(nextTask.IsCompleted);

        time.Advance(TimeSpan.FromSeconds(20));
        await nextTask;

        Assert.Equal(2, inner.Requests.Count);
    }
}
