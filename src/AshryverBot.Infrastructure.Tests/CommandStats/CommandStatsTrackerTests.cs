using AshryverBot.Infrastructure.CommandStats;
using Microsoft.Extensions.Time.Testing;
using Xunit;

namespace AshryverBot.Infrastructure.Tests.CommandStats;

public class CommandStatsTrackerTests
{
    [Fact]
    public void Initial_state_is_zero()
    {
        var tracker = new CommandStatsTracker(new FakeTimeProvider());

        Assert.Equal(0, tracker.ExecutedToday);
    }

    [Fact]
    public void RecordExecution_increments_count()
    {
        var tracker = new CommandStatsTracker(new FakeTimeProvider());

        tracker.RecordExecution();
        tracker.RecordExecution();
        tracker.RecordExecution();

        Assert.Equal(3, tracker.ExecutedToday);
    }

    [Fact]
    public void Count_resets_when_utc_day_changes()
    {
        var time = new FakeTimeProvider(new DateTimeOffset(2026, 5, 17, 22, 0, 0, TimeSpan.Zero));
        var tracker = new CommandStatsTracker(time);

        tracker.RecordExecution();
        tracker.RecordExecution();
        Assert.Equal(2, tracker.ExecutedToday);

        time.Advance(TimeSpan.FromHours(3));

        Assert.Equal(0, tracker.ExecutedToday);
    }

    [Fact]
    public void Reading_count_after_day_change_then_recording_starts_fresh()
    {
        var time = new FakeTimeProvider(new DateTimeOffset(2026, 5, 17, 23, 0, 0, TimeSpan.Zero));
        var tracker = new CommandStatsTracker(time);

        tracker.RecordExecution();
        tracker.RecordExecution();

        time.Advance(TimeSpan.FromHours(2));
        tracker.RecordExecution();

        Assert.Equal(1, tracker.ExecutedToday);
    }

    [Fact]
    public void Day_change_during_record_resets_before_increment()
    {
        var time = new FakeTimeProvider(new DateTimeOffset(2026, 5, 17, 23, 59, 0, TimeSpan.Zero));
        var tracker = new CommandStatsTracker(time);

        tracker.RecordExecution();
        tracker.RecordExecution();

        time.Advance(TimeSpan.FromMinutes(2));
        tracker.RecordExecution();

        Assert.Equal(1, tracker.ExecutedToday);
    }
}
