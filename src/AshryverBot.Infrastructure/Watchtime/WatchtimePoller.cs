using AshryverBot.Database.Repositories;
using AshryverBot.Infrastructure.StreamStats;
using AshryverBot.Infrastructure.Twitch.Tokens;
using AshryverBot.Twitch.Configuration;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Models.Chat.GetChatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AshryverBot.Infrastructure.Watchtime;

public class WatchtimePoller(
    IServiceScopeFactory scopeFactory,
    IOptions<TwitchOptions> twitchOptions,
    IOptions<WatchtimePollerOptions> pollerOptions,
    IStreamStatsWriter streamStats,
    TimeProvider timeProvider,
    ILogger<WatchtimePoller> logger) : BackgroundService
{
    private readonly TwitchOptions _twitchOptions = twitchOptions.Value;
    private readonly WatchtimePollerOptions _pollerOptions = pollerOptions.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (string.IsNullOrWhiteSpace(_twitchOptions.BroadcasterId))
        {
            logger.LogWarning("Twitch:BroadcasterId is not configured — watchtime polling disabled.");
            return;
        }

        if (string.IsNullOrWhiteSpace(_twitchOptions.BotUserId))
        {
            logger.LogWarning("Twitch:BotUserId is not configured — watchtime polling disabled.");
            return;
        }

        var interval = _pollerOptions.Interval;
        if (interval <= TimeSpan.Zero) interval = TimeSpan.FromMinutes(1);

        logger.LogInformation("Watchtime poller starting with interval {Interval}.", interval);

        using var timer = new PeriodicTimer(interval, timeProvider);
        try
        {
            do
            {
                try
                {
                    await TickAsync((long)interval.TotalSeconds, stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Watchtime poll tick failed.");
                }
            }
            while (await timer.WaitForNextTickAsync(stoppingToken));
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // graceful shutdown
        }
    }

    private async Task TickAsync(long secondsToAdd, CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var refresher = scope.ServiceProvider.GetRequiredService<ITwitchTokenRefresher>();
        var streams = scope.ServiceProvider.GetRequiredService<IStreamsApi>();
        var chatApi = scope.ServiceProvider.GetRequiredService<IChatApi>();
        var repo = scope.ServiceProvider.GetRequiredService<IWatchtimeRepository>();

        var botUserId = _twitchOptions.BotUserId!;
        var broadcasterId = _twitchOptions.BroadcasterId!;

        var token = await refresher.GetValidAsync(botUserId, cancellationToken);
        if (token is null)
        {
            logger.LogDebug("Bot token unavailable; skipping watchtime tick.");
            return;
        }

        var streamResponse = await streams.GetStreamsAsync(
            token.AccessToken,
            userIds: [broadcasterId],
            cancellationToken: cancellationToken);

        if (streamResponse.Data.Count == 0)
        {
            streamStats.MarkOffline();
            logger.LogTrace("Stream offline; skipping watchtime tick.");
            return;
        }

        streamStats.Update(streamResponse.Data.First().ViewerCount);

        var chatters = new List<WatchtimeChatter>();
        string? cursor = null;
        do
        {
            var page = await chatApi.GetChattersAsync(
                token.AccessToken,
                broadcasterId,
                botUserId,
                first: 1000,
                after: cursor,
                cancellationToken: cancellationToken);

            foreach (var chatter in page.Data)
            {
                if (string.IsNullOrWhiteSpace(chatter.UserId) || string.IsNullOrWhiteSpace(chatter.UserLogin))
                    continue;
                chatters.Add(new WatchtimeChatter(chatter.UserId, chatter.UserLogin, chatter.UserName));
            }

            cursor = page.Pagination?.Cursor;
        }
        while (!string.IsNullOrWhiteSpace(cursor));

        if (chatters.Count == 0)
        {
            logger.LogTrace("Stream live but no chatters returned.");
            return;
        }

        var now = timeProvider.GetUtcNow();
        var touched = await repo.AddSecondsAsync(chatters, secondsToAdd, now, cancellationToken);
        logger.LogDebug("Added {Seconds}s of watchtime to {Count} chatters (rows touched: {Touched}).",
            secondsToAdd, chatters.Count, touched);
    }
}
