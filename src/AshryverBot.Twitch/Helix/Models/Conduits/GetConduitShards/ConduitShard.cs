using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.EventSub.Common;

namespace AshryverBot.Twitch.Helix.Models.Conduits.GetConduitShards;

public record ConduitShard(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("transport")] EventSubTransport Transport
);
