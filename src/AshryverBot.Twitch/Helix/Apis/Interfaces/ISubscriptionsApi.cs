using AshryverBot.Twitch.Helix.Models.Subscriptions.CheckUserSubscription;
using AshryverBot.Twitch.Helix.Models.Subscriptions.GetBroadcasterSubscriptions;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface ISubscriptionsApi
{
    Task<GetBroadcasterSubscriptionsResponse> GetBroadcasterSubscriptionsAsync(
        string accessToken,
        string broadcasterId,
        IEnumerable<string>? userIds = null,
        int? first = null,
        string? after = null,
        string? before = null,
        CancellationToken cancellationToken = default);

    Task<CheckUserSubscriptionResponse> CheckUserSubscriptionAsync(
        string accessToken,
        string broadcasterId,
        string userId,
        CancellationToken cancellationToken = default);
}
