using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Conduits.GetConduitShards;

namespace AshryverBot.Twitch.Helix.Models.Conduits.UpdateConduitShards;

public record UpdateConduitShardsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ConduitShard> Data,
    [property: JsonPropertyName("errors")] IReadOnlyCollection<UpdateConduitShardError> Errors
);

public record UpdateConduitShardError(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("message")] string Message,
    [property: JsonPropertyName("code")] string Code
);
