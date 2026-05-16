namespace AshryverBot.Infrastructure.StreamStats;

public interface IStreamStats
{
    bool IsLive { get; }
    int? ViewerCount { get; }
    int? ViewerDeltaLastHour { get; }
}
