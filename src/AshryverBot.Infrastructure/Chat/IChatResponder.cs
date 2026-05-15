namespace AshryverBot.Infrastructure.Chat;

public interface IChatResponder
{
    Task ReplyAsync(string broadcasterId, string message, string? replyToMessageId = null, CancellationToken cancellationToken = default);
}
