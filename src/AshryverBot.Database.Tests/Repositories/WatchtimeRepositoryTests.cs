using AshryverBot.Database.Entities;
using AshryverBot.Database.Repositories;
using AshryverBot.Database.Repositories.Interfaces;
using AshryverBot.Database.Tests.TestSupport;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AshryverBot.Database.Tests.Repositories;

public class WatchtimeRepositoryTests : IDisposable
{
    private readonly SqliteDbContextFactory _factory = new();
    private readonly WatchtimeRepository _repository;

    public WatchtimeRepositoryTests()
    {
        _repository = new WatchtimeRepository(_factory);
    }

    [Fact]
    public async Task AddSecondsAsync_no_chatters_returns_zero_and_writes_nothing()
    {
        var result = await _repository.AddSecondsAsync([], 60, DateTimeOffset.UtcNow);

        Assert.Equal(0, result);
        await using var db = await _factory.CreateDbContextAsync();
        Assert.Empty(db.Watchtimes);
    }

    [Fact]
    public async Task AddSecondsAsync_non_positive_seconds_returns_zero_and_writes_nothing()
    {
        var chatters = new[] { new WatchtimeChatter("u1", "alice", "Alice") };

        var zero = await _repository.AddSecondsAsync(chatters, 0, DateTimeOffset.UtcNow);
        var negative = await _repository.AddSecondsAsync(chatters, -10, DateTimeOffset.UtcNow);

        Assert.Equal(0, zero);
        Assert.Equal(0, negative);
        await using var db = await _factory.CreateDbContextAsync();
        Assert.Empty(db.Watchtimes);
    }

    [Fact]
    public async Task AddSecondsAsync_inserts_new_chatters_with_initial_state()
    {
        var now = new DateTimeOffset(2026, 5, 17, 12, 0, 0, TimeSpan.Zero);
        var chatters = new[]
        {
            new WatchtimeChatter("u1", "alice", "Alice"),
            new WatchtimeChatter("u2", "bob", null),
        };

        var touched = await _repository.AddSecondsAsync(chatters, 120, now);

        Assert.Equal(2, touched);
        await using var db = await _factory.CreateDbContextAsync();
        var rows = await db.Watchtimes.AsNoTracking().OrderBy(x => x.Login).ToListAsync();
        Assert.Collection(rows,
            r =>
            {
                Assert.Equal("u1", r.TwitchUserId);
                Assert.Equal("alice", r.Login);
                Assert.Equal("Alice", r.DisplayName);
                Assert.Equal(120, r.TotalSeconds);
                Assert.Equal(now, r.LastSeenAt);
                Assert.Equal(now, r.CreatedAt);
                Assert.Equal(now, r.UpdatedAt);
            },
            r =>
            {
                Assert.Equal("u2", r.TwitchUserId);
                Assert.Equal("bob", r.Login);
                Assert.Null(r.DisplayName);
                Assert.Equal(120, r.TotalSeconds);
            });
    }

    [Fact]
    public async Task AddSecondsAsync_accumulates_on_existing_chatter_and_preserves_created_at()
    {
        var createdAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var firstTick = new DateTimeOffset(2026, 5, 17, 12, 0, 0, TimeSpan.Zero);
        var secondTick = firstTick.AddMinutes(1);

        await using (var seed = await _factory.CreateDbContextAsync())
        {
            seed.Watchtimes.Add(new WatchtimeEntity
            {
                TwitchUserId = "u1",
                Login = "alice",
                DisplayName = "Alice",
                TotalSeconds = 300,
                LastSeenAt = createdAt,
                CreatedAt = createdAt,
                UpdatedAt = createdAt,
            });
            await seed.SaveChangesAsync();
        }

        var touched1 = await _repository.AddSecondsAsync(
            [new WatchtimeChatter("u1", "alice", "Alice")], 60, firstTick);
        var touched2 = await _repository.AddSecondsAsync(
            [new WatchtimeChatter("u1", "alice", "Alice")], 60, secondTick);

        Assert.Equal(1, touched1);
        Assert.Equal(1, touched2);

        await using var db = await _factory.CreateDbContextAsync();
        var row = await db.Watchtimes.AsNoTracking().SingleAsync();
        Assert.Equal(300 + 60 + 60, row.TotalSeconds);
        Assert.Equal(createdAt, row.CreatedAt);
        Assert.Equal(secondTick, row.LastSeenAt);
        Assert.Equal(secondTick, row.UpdatedAt);
    }

    [Fact]
    public async Task AddSecondsAsync_refreshes_login_and_display_name_for_existing_chatter()
    {
        var seedTime = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        await using (var seed = await _factory.CreateDbContextAsync())
        {
            seed.Watchtimes.Add(new WatchtimeEntity
            {
                TwitchUserId = "u1",
                Login = "old_login",
                DisplayName = "OldName",
                TotalSeconds = 0,
                LastSeenAt = seedTime,
                CreatedAt = seedTime,
                UpdatedAt = seedTime,
            });
            await seed.SaveChangesAsync();
        }

        await _repository.AddSecondsAsync(
            [new WatchtimeChatter("u1", "new_login", "NewName")], 30, seedTime.AddDays(1));

        await using var db = await _factory.CreateDbContextAsync();
        var row = await db.Watchtimes.AsNoTracking().SingleAsync();
        Assert.Equal("new_login", row.Login);
        Assert.Equal("NewName", row.DisplayName);
    }

    [Fact]
    public async Task AddSecondsAsync_handles_mixed_new_and_existing_chatters_in_one_call()
    {
        var seedTime = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var tickTime = new DateTimeOffset(2026, 5, 17, 12, 0, 0, TimeSpan.Zero);

        await using (var seed = await _factory.CreateDbContextAsync())
        {
            seed.Watchtimes.Add(new WatchtimeEntity
            {
                TwitchUserId = "u1",
                Login = "alice",
                DisplayName = "Alice",
                TotalSeconds = 200,
                LastSeenAt = seedTime,
                CreatedAt = seedTime,
                UpdatedAt = seedTime,
            });
            await seed.SaveChangesAsync();
        }

        var touched = await _repository.AddSecondsAsync(
            [
                new WatchtimeChatter("u1", "alice", "Alice"),
                new WatchtimeChatter("u2", "bob", "Bob"),
            ],
            60,
            tickTime);

        Assert.Equal(2, touched);

        await using var db = await _factory.CreateDbContextAsync();
        var rows = await db.Watchtimes.AsNoTracking().OrderBy(x => x.TwitchUserId).ToListAsync();
        Assert.Equal(2, rows.Count);
        Assert.Equal(260, rows[0].TotalSeconds);
        Assert.Equal(60, rows[1].TotalSeconds);
        Assert.Equal(seedTime, rows[0].CreatedAt);
        Assert.Equal(tickTime, rows[1].CreatedAt);
    }

    public void Dispose() => _factory.Dispose();
}
