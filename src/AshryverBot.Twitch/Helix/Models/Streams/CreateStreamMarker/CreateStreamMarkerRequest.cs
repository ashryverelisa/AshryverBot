using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Streams.CreateStreamMarker;

public record CreateStreamMarkerRequest(
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("description")] string? Description
);
