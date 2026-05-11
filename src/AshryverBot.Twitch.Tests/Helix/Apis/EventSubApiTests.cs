using System.Text.Json;
using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.EventSub.Common;
using AshryverBot.Twitch.Helix.Models.EventSub.CreateEventSubSubscription;
using AshryverBot.Twitch.Helix.Models.EventSub.GetEventSubSubscriptions;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class EventSubApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly EventSubApi _api;
    public EventSubApiTests() => _api = new EventSubApi(_client);

    [Fact]
    public async Task CreateEventSubSubscriptionAsync_uses_POST_with_body_no_query()
    {
        _client.PostAsync<CreateEventSubSubscriptionRequest, CreateEventSubSubscriptionResponse>(
            null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new CreateEventSubSubscriptionResponse([], 0, 0, 0));

        var transport = new EventSubTransport("webhook", "cb", "s", null, null, null, null);
        var body = new CreateEventSubSubscriptionRequest("channel.follow", "2", JsonDocument.Parse("{}").RootElement, transport);
        await _api.CreateEventSubSubscriptionAsync("tok", body);

        await _client.Received(1).PostAsync<CreateEventSubSubscriptionRequest, CreateEventSubSubscriptionResponse>(
            "eventsub/subscriptions", "tok", body, null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteEventSubSubscriptionAsync_uses_DELETE_with_id()
    {
        await _api.DeleteEventSubSubscriptionAsync("tok", "sub");

        await _client.Received(1).DeleteAsync(
            "eventsub/subscriptions", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("id", "sub") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetEventSubSubscriptionsAsync_no_params_sends_empty_query()
    {
        _client.GetAsync<GetEventSubSubscriptionsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetEventSubSubscriptionsResponse([], 0, 0, 0, null));

        await _api.GetEventSubSubscriptionsAsync("tok");

        await _client.Received(1).GetAsync<GetEventSubSubscriptionsResponse>(
            "eventsub/subscriptions", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.TotalCount() == 0),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetEventSubSubscriptionsAsync_all_filters()
    {
        _client.GetAsync<GetEventSubSubscriptionsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetEventSubSubscriptionsResponse([], 0, 0, 0, null));

        await _api.GetEventSubSubscriptionsAsync("tok",
            status: "enabled", type: "channel.follow", userId: "u", subscriptionId: "s", after: "cur");

        await _client.Received(1).GetAsync<GetEventSubSubscriptionsResponse>(
            "eventsub/subscriptions", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("status", "enabled") && q.Has("type", "channel.follow")
                && q.Has("user_id", "u") && q.Has("subscription_id", "s")
                && q.Has("after", "cur")),
            Arg.Any<CancellationToken>());
    }
}
