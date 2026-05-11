using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Streams.CreateStreamMarker;

public record StreamMarker(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("created_at")] DateTimeOffset CreatedAt,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("position_seconds")] int PositionSeconds
);
