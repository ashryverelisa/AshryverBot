using AshryverBot.Infrastructure.CommandStats.Interfaces;

namespace AshryverBot.Infrastructure.CommandStats;

internal class CommandStatsTracker(TimeProvider timeProvider) : ICommandStats, ICommandStatsWriter
{
    private readonly Lock _gate = new();
    private DateOnly _day;
    private int _count;

    public int ExecutedToday
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

    public void RecordExecution()
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
