using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.EventSub.Common;

namespace AshryverBot.Twitch.Helix.Models.Conduits.UpdateConduitShards;

public record UpdateConduitShardsRequest(
    [property: JsonPropertyName("conduit_id")] string ConduitId,
    [property: JsonPropertyName("shards")] IReadOnlyCollection<UpdateConduitShard> Shards
);

public record UpdateConduitShard(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("transport")] EventSubTransport Transport
);
