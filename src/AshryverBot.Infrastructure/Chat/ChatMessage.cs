namespace AshryverBot.Infrastructure.Chat;

public record ChatMessage(
    string BroadcasterUserId,
    string ChatterUserId,
    string ChatterUserLogin,
    string ChatterDisplayName,
    string MessageId,
    string Text);
