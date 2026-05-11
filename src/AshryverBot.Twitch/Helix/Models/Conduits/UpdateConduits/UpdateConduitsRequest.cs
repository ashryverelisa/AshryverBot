using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Conduits.UpdateConduits;

public record UpdateConduitsRequest(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("shard_count")] int ShardCount
);
