using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.EventSub.CreateEventSubSubscription;
using AshryverBot.Twitch.Helix.Models.EventSub.GetEventSubSubscriptions;

namespace AshryverBot.Twitch.Helix.Apis;

public class EventSubApi(ITwitchClient client) : IEventSubApi
{
    public Task<CreateEventSubSubscriptionResponse> CreateEventSubSubscriptionAsync(
        string accessToken,
        CreateEventSubSubscriptionRequest body,
        CancellationToken cancellationToken = default)
        => client.PostAsync<CreateEventSubSubscriptionRequest, CreateEventSubSubscriptionResponse>(
            "eventsub/subscriptions", accessToken, body, queryParameters: null, cancellationToken);

    public Task DeleteEventSubSubscriptionAsync(
        string accessToken,
        string id,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("id", id),
        };
        return client.DeleteAsync("eventsub/subscriptions", accessToken, query, cancellationToken);
    }

    public Task<GetEventSubSubscriptionsResponse> GetEventSubSubscriptionsAsync(
        string accessToken,
        string? status = null,
        string? type = null,
        string? userId = null,
        string? subscriptionId = null,
        string? after = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddIfNotNull("status", status);
        query.AddIfNotNull("type", type);
        query.AddIfNotNull("user_id", userId);
        query.AddIfNotNull("subscription_id", subscriptionId);
        query.AddIfNotNull("after", after);
        return client.GetAsync<GetEventSubSubscriptionsResponse>("eventsub/subscriptions", accessToken, query, cancellationToken);
    }
}
