using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Streams.CreateStreamMarker;

public record CreateStreamMarkerResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<StreamMarker> Data
);
