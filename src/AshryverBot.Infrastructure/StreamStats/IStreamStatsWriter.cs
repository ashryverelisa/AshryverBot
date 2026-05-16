namespace AshryverBot.Infrastructure.StreamStats;

public interface IStreamStatsWriter
{
    void Update(int viewerCount);
    void MarkOffline();
}
