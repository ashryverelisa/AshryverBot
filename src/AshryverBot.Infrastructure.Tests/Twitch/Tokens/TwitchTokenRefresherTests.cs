using AshryverBot.Database.Entities;
using AshryverBot.Database.Repositories.Interfaces;
using AshryverBot.Infrastructure.Twitch.Tokens;
using AshryverBot.Twitch.Auth;
using AshryverBot.Twitch.Clients.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using NSubstitute;
using Xunit;

namespace AshryverBot.Infrastructure.Tests.Twitch.Tokens;

public class TwitchTokenRefresherTests
{
    private readonly ITwitchTokenRepository _repository = Substitute.For<ITwitchTokenRepository>();
    private readonly ITwitchOAuthClient _oauthClient = Substitute.For<ITwitchOAuthClient>();
    private readonly FakeTimeProvider _time = new(new DateTimeOffset(2026, 5, 17, 12, 0, 0, TimeSpan.Zero));
    private readonly TwitchTokenRefresher _refresher;

    public TwitchTokenRefresherTests()
    {
        _refresher = new TwitchTokenRefresher(
            _repository,
            _oauthClient,
            _time,
            NullLogger<TwitchTokenRefresher>.Instance);
    }

    private TwitchTokenEntity SeedEntity(DateTimeOffset expiresAt) => new()
    {
        TwitchUserId = "u1",
        Login = "user",
        AccessToken = "old-access",
        RefreshToken = "old-refresh",
        ExpiresAt = expiresAt,
        Scopes = "chat:read chat:edit",
        CreatedAt = _time.GetUtcNow(),
        UpdatedAt = _time.GetUtcNow(),
    };

    [Fact]
    public async Task GetValidAsync_returns_null_when_no_token_exists()
    {
        _repository.GetAsync("u1", Arg.Any<CancellationToken>()).Returns((TwitchTokenEntity?)null);

        var result = await _refresher.GetValidAsync("u1");

        Assert.Null(result);
        await _oauthClient.DidNotReceiveWithAnyArgs().RefreshAsync(default!, default);
    }

    [Fact]
    public async Task GetValidAsync_returns_token_unchanged_when_not_close_to_expiry()
    {
        var entity = SeedEntity(_time.GetUtcNow().AddMinutes(10));
        _repository.GetAsync("u1", Arg.Any<CancellationToken>()).Returns(entity);

        var result = await _refresher.GetValidAsync("u1");

        Assert.NotNull(result);
        Assert.Equal("old-access", result!.AccessToken);
        await _oauthClient.DidNotReceiveWithAnyArgs().RefreshAsync(default!, default);
    }

    [Fact]
    public async Task GetValidAsync_refreshes_when_within_safety_window()
    {
        var entity = SeedEntity(_time.GetUtcNow().AddMinutes(4));
        _repository.GetAsync("u1", Arg.Any<CancellationToken>()).Returns(entity);
        _oauthClient.RefreshAsync("old-refresh", Arg.Any<CancellationToken>())
            .Returns(new RefreshResponse("new-access", "new-refresh", 3600, ["chat:read"]));

        var result = await _refresher.GetValidAsync("u1");

        Assert.NotNull(result);
        Assert.Equal("new-access", result!.AccessToken);
        Assert.Equal("new-refresh", result.RefreshToken);
        Assert.Equal(_time.GetUtcNow().AddSeconds(3600), result.ExpiresAt);
        Assert.Equal(["chat:read"], result.Scopes);
    }

    [Fact]
    public async Task GetValidAsync_refreshes_when_already_expired()
    {
        var entity = SeedEntity(_time.GetUtcNow().AddMinutes(-1));
        _repository.GetAsync("u1", Arg.Any<CancellationToken>()).Returns(entity);
        _oauthClient.RefreshAsync("old-refresh", Arg.Any<CancellationToken>())
            .Returns(new RefreshResponse("new-access", "new-refresh", 3600, ["chat:read"]));

        var result = await _refresher.GetValidAsync("u1");

        Assert.NotNull(result);
        Assert.Equal("new-access", result!.AccessToken);
        await _oauthClient.Received(1).RefreshAsync("old-refresh", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetValidAsync_returns_null_when_oauth_refresh_throws()
    {
        var entity = SeedEntity(_time.GetUtcNow().AddMinutes(1));
        _repository.GetAsync("u1", Arg.Any<CancellationToken>()).Returns(entity);
        _oauthClient.RefreshAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns<Task<RefreshResponse>>(_ => throw new HttpRequestException("twitch unreachable"));

        var result = await _refresher.GetValidAsync("u1");

        Assert.Null(result);
    }

    [Fact]
    public async Task RefreshAsync_persists_updated_token_via_repository()
    {
        var entity = SeedEntity(_time.GetUtcNow().AddMinutes(1));
        _repository.GetAsync("u1", Arg.Any<CancellationToken>()).Returns(entity);
        _oauthClient.RefreshAsync("old-refresh", Arg.Any<CancellationToken>())
            .Returns(new RefreshResponse("new-access", "new-refresh", 7200, ["chat:read", "chat:edit"]));

        var result = await _refresher.RefreshAsync(entity.ToInfo());

        Assert.Equal("new-access", result.AccessToken);
        Assert.Equal(_time.GetUtcNow().AddSeconds(7200), result.ExpiresAt);
        await _repository.Received(1).UpdateAsync(
            Arg.Is<TwitchTokenEntity>(e =>
                e.TwitchUserId == "u1" &&
                e.AccessToken == "new-access" &&
                e.RefreshToken == "new-refresh"),
            Arg.Any<CancellationToken>());
    }
}
