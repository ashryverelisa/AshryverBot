using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.Subscriptions.CheckUserSubscription;
using AshryverBot.Twitch.Helix.Models.Subscriptions.GetBroadcasterSubscriptions;

namespace AshryverBot.Twitch.Helix.Apis;

public class SubscriptionsApi(ITwitchClient client) : ISubscriptionsApi
{
    public Task<GetBroadcasterSubscriptionsResponse> GetBroadcasterSubscriptionsAsync(
        string accessToken,
        string broadcasterId,
        IEnumerable<string>? userIds = null,
        int? first = null,
        string? after = null,
        string? before = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        query.AddMany("user_id", userIds);
        query.AddIfNotNull("first", first);
        query.AddIfNotNull("after", after);
        query.AddIfNotNull("before", before);
        return client.GetAsync<GetBroadcasterSubscriptionsResponse>("subscriptions", accessToken, query, cancellationToken);
    }

    public Task<CheckUserSubscriptionResponse> CheckUserSubscriptionAsync(
        string accessToken,
        string broadcasterId,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("user_id", userId),
        };
        return client.GetAsync<CheckUserSubscriptionResponse>("subscriptions/user", accessToken, query, cancellationToken);
    }
}
