using AshryverBot.Infrastructure.Chat.Commands;

namespace AshryverBot.Infrastructure.Chat.Interfaces;

public interface IChatCommand
{
    /// <summary>
    /// Command name, without the leading "!". Case-insensitive.
    /// </summary>
    string Name { get; }

    Task ExecuteAsync(ChatCommandContext context, CancellationToken cancellationToken);
}