namespace AshryverBot.Infrastructure.Chat;

public interface IChatCommand
{
    /// <summary>
    /// Command name, without the leading "!". Case-insensitive.
    /// </summary>
    string Name { get; }

    Task ExecuteAsync(ChatCommandContext context, CancellationToken cancellationToken);
}

public record ChatCommandContext(
    ChatMessage Message,
    IReadOnlyList<string> Arguments);
