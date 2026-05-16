using AshryverBot.Infrastructure.StreamStats;
using Microsoft.Extensions.Time.Testing;
using Xunit;

namespace AshryverBot.Infrastructure.Tests.StreamStats;

public class StreamStatsTrackerTests
{
    [Fact]
    public void Initial_state_is_offline_and_empty()
    {
        var tracker = new StreamStatsTracker(new FakeTimeProvider());

        Assert.False(tracker.IsLive);
        Assert.Null(tracker.ViewerCount);
        Assert.Null(tracker.ViewerDeltaLastHour);
    }

    [Fact]
    public void Update_marks_live_and_records_count()
    {
        var tracker = new StreamStatsTracker(new FakeTimeProvider());

        tracker.Update(142);

        Assert.True(tracker.IsLive);
        Assert.Equal(142, tracker.ViewerCount);
    }

    [Fact]
    public void MarkOffline_clears_state_and_drops_history()
    {
        var time = new FakeTimeProvider();
        var tracker = new StreamStatsTracker(time);
        tracker.Update(100);
        time.Advance(TimeSpan.FromMinutes(60));
        tracker.Update(120);

        tracker.MarkOffline();

        Assert.False(tracker.IsLive);
        Assert.Null(tracker.ViewerCount);
        Assert.Null(tracker.ViewerDeltaLastHour);
    }

    [Fact]
    public void Delta_returns_null_when_no_sample_old_enough()
    {
        var time = new FakeTimeProvider();
        var tracker = new StreamStatsTracker(time);

        tracker.Update(100);
        time.Advance(TimeSpan.FromMinutes(30));
        tracker.Update(110);

        Assert.Null(tracker.ViewerDeltaLastHour);
    }

    [Fact]
    public void Delta_returns_difference_between_now_and_oldest_sample_in_window()
    {
        var time = new FakeTimeProvider();
        var tracker = new StreamStatsTracker(time);

        tracker.Update(100);
        time.Advance(TimeSpan.FromMinutes(56));
        tracker.Update(125);

        Assert.Equal(25, tracker.ViewerDeltaLastHour);
    }

    [Fact]
    public void Delta_handles_negative_change()
    {
        var time = new FakeTimeProvider();
        var tracker = new StreamStatsTracker(time);

        tracker.Update(200);
        time.Advance(TimeSpan.FromMinutes(60));
        tracker.Update(180);

        Assert.Equal(-20, tracker.ViewerDeltaLastHour);
    }

    [Fact]
    public void Samples_older_than_retention_are_pruned_and_no_longer_used_for_delta()
    {
        var time = new FakeTimeProvider();
        var tracker = new StreamStatsTracker(time);

        tracker.Update(50);
        time.Advance(TimeSpan.FromMinutes(91));
        tracker.Update(80);

        Assert.Null(tracker.ViewerDeltaLastHour);
        Assert.Equal(80, tracker.ViewerCount);
    }

    [Fact]
    public void Going_offline_then_live_again_resets_delta_history()
    {
        var time = new FakeTimeProvider();
        var tracker = new StreamStatsTracker(time);

        tracker.Update(100);
        time.Advance(TimeSpan.FromMinutes(60));
        tracker.Update(150);
        Assert.NotNull(tracker.ViewerDeltaLastHour);

        tracker.MarkOffline();
        tracker.Update(150);

        Assert.Null(tracker.ViewerDeltaLastHour);
    }
}
