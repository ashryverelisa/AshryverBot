using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Streams.GetStreamKey;

public record GetStreamKeyResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<StreamKey> Data
);
