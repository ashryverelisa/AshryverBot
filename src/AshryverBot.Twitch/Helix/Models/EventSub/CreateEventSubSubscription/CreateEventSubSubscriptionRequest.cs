using System.Text.Json;
using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.EventSub.Common;

namespace AshryverBot.Twitch.Helix.Models.EventSub.CreateEventSubSubscription;

public record CreateEventSubSubscriptionRequest(
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("version")] string Version,
    [property: JsonPropertyName("condition")] JsonElement Condition,
    [property: JsonPropertyName("transport")] EventSubTransport Transport
);
