using AshryverBot.Infrastructure.FollowerStats.Interfaces;

namespace AshryverBot.Infrastructure.FollowerStats;

internal class FollowerStatsTracker(TimeProvider timeProvider) : IFollowerStats, IFollowerStatsWriter
{
    private readonly Lock _gate = new();
    private DateOnly _day;
    private int _count;

    public int NewToday
    {
        get
        {
            lock (_gate)
            {
                RollIfNewDay();
                return _count;
            }
        }
    }

    public void RecordFollow()
    {
        lock (_gate)
        {
            RollIfNewDay();
            _count++;
        }
    }

    private void RollIfNewDay()
    {
        var today = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);
        if (today == _day) return;
        _day = today;
        _count = 0;
    }
}
