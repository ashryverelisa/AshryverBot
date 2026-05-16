namespace AshryverBot.Twitch.EventSub.WebSocket.Interfaces;

public interface IEventSubConnectionObserver
{
    Task OnConnectedAsync(CancellationToken cancellationToken);
    Task OnDisconnectedAsync(CancellationToken cancellationToken);
}
