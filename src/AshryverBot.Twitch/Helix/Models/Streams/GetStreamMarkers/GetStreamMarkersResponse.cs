using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Streams.GetStreamMarkers;

public record GetStreamMarkersResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<StreamMarkerUserGroup> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
