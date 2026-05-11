using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Subscriptions.GetBroadcasterSubscriptions;

public record GetBroadcasterSubscriptionsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<BroadcasterSubscription> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination,
    [property: JsonPropertyName("total")] int Total,
    [property: JsonPropertyName("points")] int Points
);
