using AshryverBot.Infrastructure.BotStatus;
using Microsoft.Extensions.Time.Testing;
using Xunit;

namespace AshryverBot.Infrastructure.Tests.BotStatus;

public class BotStatusTrackerTests
{
    [Fact]
    public void Initial_state_is_offline()
    {
        var tracker = new BotStatusTracker(new FakeTimeProvider());

        Assert.False(tracker.IsOnline);
        Assert.Null(tracker.ConnectedSince);
    }

    [Fact]
    public async Task OnConnected_marks_online_and_records_timestamp()
    {
        var time = new FakeTimeProvider(new DateTimeOffset(2026, 5, 17, 12, 0, 0, TimeSpan.Zero));
        var tracker = new BotStatusTracker(time);

        await tracker.OnConnectedAsync(CancellationToken.None);

        Assert.True(tracker.IsOnline);
        Assert.Equal(time.GetUtcNow(), tracker.ConnectedSince);
    }

    [Fact]
    public async Task Repeated_OnConnected_preserves_original_timestamp()
    {
        var start = new DateTimeOffset(2026, 5, 17, 12, 0, 0, TimeSpan.Zero);
        var time = new FakeTimeProvider(start);
        var tracker = new BotStatusTracker(time);

        await tracker.OnConnectedAsync(CancellationToken.None);
        time.Advance(TimeSpan.FromMinutes(10));
        await tracker.OnConnectedAsync(CancellationToken.None);

        Assert.Equal(start, tracker.ConnectedSince);
    }

    [Fact]
    public async Task OnDisconnected_clears_state()
    {
        var tracker = new BotStatusTracker(new FakeTimeProvider());

        await tracker.OnConnectedAsync(CancellationToken.None);
        await tracker.OnDisconnectedAsync(CancellationToken.None);

        Assert.False(tracker.IsOnline);
        Assert.Null(tracker.ConnectedSince);
    }

    [Fact]
    public async Task Reconnecting_after_disconnect_uses_new_timestamp()
    {
        var time = new FakeTimeProvider(new DateTimeOffset(2026, 5, 17, 12, 0, 0, TimeSpan.Zero));
        var tracker = new BotStatusTracker(time);

        await tracker.OnConnectedAsync(CancellationToken.None);
        await tracker.OnDisconnectedAsync(CancellationToken.None);
        time.Advance(TimeSpan.FromMinutes(5));
        await tracker.OnConnectedAsync(CancellationToken.None);

        Assert.Equal(time.GetUtcNow(), tracker.ConnectedSince);
    }
}
