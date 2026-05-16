using AshryverBot.Twitch.Configuration;
using AshryverBot.Twitch.EventSub.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AshryverBot.Infrastructure.EventSub;

public class EventSubWebSocketHostedService(
    EventSubWebSocketClient client,
    IEnumerable<IEventSubConnectionObserver> connectionObservers,
    IOptions<TwitchOptions> twitchOptions,
    ILogger<EventSubWebSocketHostedService> logger) : BackgroundService
{
    private readonly TwitchOptions _twitchOptions = twitchOptions.Value;
    private readonly IReadOnlyList<IEventSubConnectionObserver> _connectionObservers = connectionObservers.ToArray();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (string.IsNullOrWhiteSpace(_twitchOptions.BroadcasterId))
        {
            logger.LogWarning("Twitch:BroadcasterId is not configured — EventSub WebSocket disabled.");
            return;
        }

        if (string.IsNullOrWhiteSpace(_twitchOptions.BotUserId))
        {
            logger.LogWarning("Twitch:BotUserId is not configured — EventSub WebSocket disabled.");
            return;
        }

        var context = new EventSubSubscriptionContext(_twitchOptions.BroadcasterId, _twitchOptions.BotUserId);
        var backoff = TimeSpan.FromSeconds(2);
        var maxBackoff = TimeSpan.FromMinutes(2);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await client.RunAsync(context, stoppingToken);
                backoff = TimeSpan.FromSeconds(2);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                await NotifyDisconnectedAsync(CancellationToken.None);
                return;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "EventSub WebSocket errored; reconnecting in {Delay}.", backoff);
                await NotifyDisconnectedAsync(CancellationToken.None);
                try
                {
                    await Task.Delay(backoff, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    return;
                }

                backoff = TimeSpan.FromSeconds(Math.Min(backoff.TotalSeconds * 2, maxBackoff.TotalSeconds));
                continue;
            }

            await NotifyDisconnectedAsync(CancellationToken.None);
        }
    }

    private async Task NotifyDisconnectedAsync(CancellationToken cancellationToken)
    {
        foreach (var observer in _connectionObservers)
        {
            try
            {
                await observer.OnDisconnectedAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "EventSub connection observer {Observer} failed in OnDisconnected.", observer.GetType().Name);
            }
        }
    }
}
