using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Conduits.GetConduits;

public record Conduit(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("shard_count")] int ShardCount
);
