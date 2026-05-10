using System.Text.Json;
using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.EventSub.Common;

public record EventSubSubscription(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("version")] string Version,
    [property: JsonPropertyName("condition")] JsonElement Condition,
    [property: JsonPropertyName("created_at")] DateTimeOffset CreatedAt,
    [property: JsonPropertyName("transport")] EventSubTransport Transport,
    [property: JsonPropertyName("cost")] int Cost
);
