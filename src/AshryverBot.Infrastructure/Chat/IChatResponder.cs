namespace AshryverBot.Infrastructure.Chat;

public interface IChatResponder
{
    Task SendAsync(string broadcasterId, string message, CancellationToken cancellationToken = default);
}
