namespace AshryverBot.Infrastructure.StreamStats.Interfaces;

public interface IStreamStats
{
    bool IsLive { get; }
    int? ViewerCount { get; }
    int? ViewerDeltaLastHour { get; }
}
