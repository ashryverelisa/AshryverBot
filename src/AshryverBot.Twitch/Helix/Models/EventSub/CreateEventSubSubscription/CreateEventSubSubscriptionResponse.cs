using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.EventSub.Common;

namespace AshryverBot.Twitch.Helix.Models.EventSub.CreateEventSubSubscription;

public record CreateEventSubSubscriptionResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<EventSubSubscription> Data,
    [property: JsonPropertyName("total")] int Total,
    [property: JsonPropertyName("total_cost")] int TotalCost,
    [property: JsonPropertyName("max_total_cost")] int MaxTotalCost
);
