namespace AshryverBot.Twitch.EventSub.WebSocket;

public interface IEventSubConnectionObserver
{
    Task OnConnectedAsync(CancellationToken cancellationToken);
    Task OnDisconnectedAsync(CancellationToken cancellationToken);
}
