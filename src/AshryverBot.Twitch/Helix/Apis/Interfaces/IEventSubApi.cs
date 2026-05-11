using AshryverBot.Twitch.Helix.Models.EventSub.CreateEventSubSubscription;
using AshryverBot.Twitch.Helix.Models.EventSub.GetEventSubSubscriptions;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IEventSubApi
{
    Task<CreateEventSubSubscriptionResponse> CreateEventSubSubscriptionAsync(
        string accessToken,
        CreateEventSubSubscriptionRequest body,
        CancellationToken cancellationToken = default);

    Task DeleteEventSubSubscriptionAsync(
        string accessToken,
        string id,
        CancellationToken cancellationToken = default);

    Task<GetEventSubSubscriptionsResponse> GetEventSubSubscriptionsAsync(
        string accessToken,
        string? status = null,
        string? type = null,
        string? userId = null,
        string? subscriptionId = null,
        string? after = null,
        CancellationToken cancellationToken = default);
}
