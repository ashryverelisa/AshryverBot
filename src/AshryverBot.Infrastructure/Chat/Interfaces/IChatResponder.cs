namespace AshryverBot.Infrastructure.Chat.Interfaces;

public interface IChatResponder
{
    Task SendAsync(string broadcasterId, string message, CancellationToken cancellationToken = default);
}
