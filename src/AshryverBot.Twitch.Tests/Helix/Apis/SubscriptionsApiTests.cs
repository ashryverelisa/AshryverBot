using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Subscriptions.CheckUserSubscription;
using AshryverBot.Twitch.Helix.Models.Subscriptions.GetBroadcasterSubscriptions;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class SubscriptionsApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly SubscriptionsApi _api;
    public SubscriptionsApiTests() => _api = new SubscriptionsApi(_client);

    [Fact]
    public async Task GetBroadcasterSubscriptionsAsync_required_only()
    {
        _client.GetAsync<GetBroadcasterSubscriptionsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetBroadcasterSubscriptionsResponse(
                [], null, 0, 0));

        await _api.GetBroadcasterSubscriptionsAsync("tok", "bc");

        await _client.Received(1).GetAsync<GetBroadcasterSubscriptionsResponse>(
            "subscriptions", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.HasNoKey("user_id")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetBroadcasterSubscriptionsAsync_all_params()
    {
        _client.GetAsync<GetBroadcasterSubscriptionsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetBroadcasterSubscriptionsResponse(
                [], null, 0, 0));

        await _api.GetBroadcasterSubscriptionsAsync("tok", "bc", ["u1", "u2"], 10, "a", "b");

        await _client.Received(1).GetAsync<GetBroadcasterSubscriptionsResponse>(
            "subscriptions", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc")
                && q.Has("user_id", "u1") && q.Has("user_id", "u2")
                && q.Has("first", "10") && q.Has("after", "a") && q.Has("before", "b")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CheckUserSubscriptionAsync_required_pair()
    {
        _client.GetAsync<CheckUserSubscriptionResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new CheckUserSubscriptionResponse([]));

        await _api.CheckUserSubscriptionAsync("tok", "bc", "u");

        await _client.Received(1).GetAsync<CheckUserSubscriptionResponse>(
            "subscriptions/user", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("user_id", "u")
                && q.TotalCount() == 2),
            Arg.Any<CancellationToken>());
    }
}
