namespace AshryverBot.Infrastructure.Chat.Commands;

public record ChatCommandContext(
    ChatMessage Message,
    IReadOnlyList<string> Arguments);