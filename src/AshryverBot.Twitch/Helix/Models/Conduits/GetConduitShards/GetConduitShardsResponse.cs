using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Conduits.GetConduitShards;

public record GetConduitShardsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ConduitShard> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
