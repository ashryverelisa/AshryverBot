namespace AshryverBot.Twitch.EventSub.WebSocket;

public interface IEventSubAccessTokenProvider
{
    Task<string> GetAccessTokenAsync(CancellationToken cancellationToken);
}
