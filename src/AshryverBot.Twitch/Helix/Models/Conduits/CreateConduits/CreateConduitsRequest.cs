using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Conduits.CreateConduits;

public record CreateConduitsRequest(
    [property: JsonPropertyName("shard_count")] int ShardCount
);
