using AshryverBot.Infrastructure.FollowerStats;
using Microsoft.Extensions.Time.Testing;
using Xunit;

namespace AshryverBot.Infrastructure.Tests.FollowerStats;

public class FollowerStatsTrackerTests
{
    [Fact]
    public void Initial_state_is_zero()
    {
        var tracker = new FollowerStatsTracker(new FakeTimeProvider());

        Assert.Equal(0, tracker.NewToday);
    }

    [Fact]
    public void RecordFollow_increments_count()
    {
        var tracker = new FollowerStatsTracker(new FakeTimeProvider());

        tracker.RecordFollow();
        tracker.RecordFollow();

        Assert.Equal(2, tracker.NewToday);
    }

    [Fact]
    public void Count_resets_when_utc_day_changes()
    {
        var time = new FakeTimeProvider(new DateTimeOffset(2026, 5, 17, 22, 0, 0, TimeSpan.Zero));
        var tracker = new FollowerStatsTracker(time);

        tracker.RecordFollow();
        tracker.RecordFollow();
        Assert.Equal(2, tracker.NewToday);

        time.Advance(TimeSpan.FromHours(3));

        Assert.Equal(0, tracker.NewToday);
    }

    [Fact]
    public void Day_change_then_record_starts_fresh()
    {
        var time = new FakeTimeProvider(new DateTimeOffset(2026, 5, 17, 23, 0, 0, TimeSpan.Zero));
        var tracker = new FollowerStatsTracker(time);

        tracker.RecordFollow();
        tracker.RecordFollow();

        time.Advance(TimeSpan.FromHours(2));
        tracker.RecordFollow();

        Assert.Equal(1, tracker.NewToday);
    }
}
