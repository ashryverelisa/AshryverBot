using AshryverBot.Database.Entities;
using AshryverBot.Database.Repositories.Interfaces;
using AshryverBot.Infrastructure.Commands;
using Microsoft.Extensions.Time.Testing;
using NSubstitute;
using Xunit;

namespace AshryverBot.Infrastructure.Tests.Commands;

public class CommandServiceTests
{
    private readonly ICommandRepository _repository = Substitute.For<ICommandRepository>();
    private readonly FakeTimeProvider _time = new(new DateTimeOffset(2026, 5, 17, 12, 0, 0, TimeSpan.Zero));

    private CommandService BuildService() => new(_repository, _time);

    private static CommandDraft SampleDraft(
        string name = "hi",
        string response = "hello",
        int cooldown = 10,
        CommandRole role = CommandRole.Everyone,
        bool enabled = true)
        => new(name, response, cooldown, role, enabled);

    [Fact]
    public async Task ListAsync_delegates_to_repository()
    {
        var expected = new List<CommandEntity>
        {
            new() { Name = "a", Response = "x" },
            new() { Name = "b", Response = "y" },
        };
        _repository.ListAsync(Arg.Any<CancellationToken>()).Returns(expected);

        var result = await BuildService().ListAsync();

        Assert.Same(expected, result);
        await _repository.Received(1).ListAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_throws_when_draft_is_null()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => BuildService().CreateAsync(null!));
    }

    [Fact]
    public async Task CreateAsync_assigns_id_timestamps_and_zero_usage()
    {
        var draft = SampleDraft();

        var created = await BuildService().CreateAsync(draft);

        Assert.NotEqual(Guid.Empty, created.Id);
        Assert.Equal(0, created.UsageCount);
        Assert.Equal(_time.GetUtcNow(), created.CreatedAt);
        Assert.Equal(_time.GetUtcNow(), created.UpdatedAt);
    }

    [Fact]
    public async Task CreateAsync_copies_draft_fields_onto_entity()
    {
        var draft = SampleDraft(
            name: "shoutout",
            response: "thanks!",
            cooldown: 30,
            role: CommandRole.Moderator,
            enabled: false);

        var created = await BuildService().CreateAsync(draft);

        Assert.Equal("shoutout", created.Name);
        Assert.Equal("thanks!", created.Response);
        Assert.Equal(30, created.CooldownSeconds);
        Assert.Equal(CommandRole.Moderator, created.RequiredRole);
        Assert.False(created.IsEnabled);
    }

    [Fact]
    public async Task CreateAsync_persists_via_repository()
    {
        var draft = SampleDraft();

        var created = await BuildService().CreateAsync(draft);

        await _repository.Received(1).CreateAsync(
            Arg.Is<CommandEntity>(e => ReferenceEquals(e, created)),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAsync_throws_when_draft_is_null()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => BuildService().UpdateAsync(Guid.NewGuid(), null!));
    }

    [Fact]
    public async Task UpdateAsync_throws_when_entity_is_missing()
    {
        var id = Guid.NewGuid();
        _repository.GetAsync(id, Arg.Any<CancellationToken>()).Returns((CommandEntity?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => BuildService().UpdateAsync(id, SampleDraft()));

        await _repository.DidNotReceiveWithAnyArgs().UpdateAsync(default!, default);
    }

    [Fact]
    public async Task UpdateAsync_applies_draft_and_refreshes_timestamp()
    {
        var id = Guid.NewGuid();
        var createdAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var existing = new CommandEntity
        {
            Id = id,
            Name = "old",
            Response = "old-response",
            CooldownSeconds = 5,
            RequiredRole = CommandRole.Everyone,
            IsEnabled = true,
            UsageCount = 42,
            CreatedAt = createdAt,
            UpdatedAt = createdAt,
        };
        _repository.GetAsync(id, Arg.Any<CancellationToken>()).Returns(existing);

        _time.Advance(TimeSpan.FromMinutes(15));
        var draft = SampleDraft(
            name: "new",
            response: "new-response",
            cooldown: 60,
            role: CommandRole.Subscriber,
            enabled: false);

        var updated = await BuildService().UpdateAsync(id, draft);

        Assert.Same(existing, updated);
        Assert.Equal(id, updated.Id);
        Assert.Equal("new", updated.Name);
        Assert.Equal("new-response", updated.Response);
        Assert.Equal(60, updated.CooldownSeconds);
        Assert.Equal(CommandRole.Subscriber, updated.RequiredRole);
        Assert.False(updated.IsEnabled);
        Assert.Equal(42, updated.UsageCount);
        Assert.Equal(createdAt, updated.CreatedAt);
        Assert.Equal(_time.GetUtcNow(), updated.UpdatedAt);
        await _repository.Received(1).UpdateAsync(existing, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SetEnabledAsync_throws_when_entity_is_missing()
    {
        var id = Guid.NewGuid();
        _repository.GetAsync(id, Arg.Any<CancellationToken>()).Returns((CommandEntity?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => BuildService().SetEnabledAsync(id, true));

        await _repository.DidNotReceiveWithAnyArgs().UpdateAsync(default!, default);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public async Task SetEnabledAsync_flips_flag_and_refreshes_timestamp(bool initial, bool target)
    {
        var id = Guid.NewGuid();
        var createdAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var existing = new CommandEntity
        {
            Id = id,
            Name = "hi",
            Response = "hello",
            IsEnabled = initial,
            CreatedAt = createdAt,
            UpdatedAt = createdAt,
        };
        _repository.GetAsync(id, Arg.Any<CancellationToken>()).Returns(existing);
        _time.Advance(TimeSpan.FromMinutes(5));

        var updated = await BuildService().SetEnabledAsync(id, target);

        Assert.Same(existing, updated);
        Assert.Equal(target, updated.IsEnabled);
        Assert.Equal(_time.GetUtcNow(), updated.UpdatedAt);
        Assert.Equal(createdAt, updated.CreatedAt);
        await _repository.Received(1).UpdateAsync(existing, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteAsync_delegates_to_repository()
    {
        var id = Guid.NewGuid();

        await BuildService().DeleteAsync(id);

        await _repository.Received(1).DeleteAsync(id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CancellationToken_is_forwarded_to_repository_calls()
    {
        var id = Guid.NewGuid();
        var existing = new CommandEntity { Id = id, Name = "n", Response = "r" };
        _repository.GetAsync(id, Arg.Any<CancellationToken>()).Returns(existing);

        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        await BuildService().UpdateAsync(id, SampleDraft(), token);

        await _repository.Received(1).GetAsync(id, token);
        await _repository.Received(1).UpdateAsync(existing, token);
    }
}
