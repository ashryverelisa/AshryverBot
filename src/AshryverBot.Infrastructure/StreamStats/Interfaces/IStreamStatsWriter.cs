namespace AshryverBot.Infrastructure.StreamStats.Interfaces;

public interface IStreamStatsWriter
{
    void Update(int viewerCount);
    void MarkOffline();
}
