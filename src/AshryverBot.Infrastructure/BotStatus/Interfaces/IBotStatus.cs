namespace AshryverBot.Infrastructure.BotStatus.Interfaces;

public interface IBotStatus
{
    bool IsOnline { get; }
    DateTimeOffset? ConnectedSince { get; }
}
