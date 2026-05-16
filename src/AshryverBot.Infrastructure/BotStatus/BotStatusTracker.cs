using AshryverBot.Infrastructure.BotStatus.Interfaces;
using AshryverBot.Twitch.EventSub.WebSocket.Interfaces;

namespace AshryverBot.Infrastructure.BotStatus;

internal class BotStatusTracker(TimeProvider timeProvider) : IBotStatus, IEventSubConnectionObserver
{
    private DateTimeOffset? _connectedSince;

    public bool IsOnline => _connectedSince.HasValue;
    public DateTimeOffset? ConnectedSince => _connectedSince;

    public Task OnConnectedAsync(CancellationToken cancellationToken)
    {
        _connectedSince ??= timeProvider.GetUtcNow();
        return Task.CompletedTask;
    }

    public Task OnDisconnectedAsync(CancellationToken cancellationToken)
    {
        _connectedSince = null;
        return Task.CompletedTask;
    }
}
