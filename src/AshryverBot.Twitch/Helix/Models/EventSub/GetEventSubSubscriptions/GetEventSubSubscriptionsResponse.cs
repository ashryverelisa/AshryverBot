using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;
using AshryverBot.Twitch.Helix.Models.EventSub.Common;

namespace AshryverBot.Twitch.Helix.Models.EventSub.GetEventSubSubscriptions;

public record GetEventSubSubscriptionsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<EventSubSubscription> Data,
    [property: JsonPropertyName("total")] int Total,
    [property: JsonPropertyName("total_cost")] int TotalCost,
    [property: JsonPropertyName("max_total_cost")] int MaxTotalCost,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
