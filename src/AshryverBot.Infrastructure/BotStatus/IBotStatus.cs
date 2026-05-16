namespace AshryverBot.Infrastructure.BotStatus;

public interface IBotStatus
{
    bool IsOnline { get; }
    DateTimeOffset? ConnectedSince { get; }
}
