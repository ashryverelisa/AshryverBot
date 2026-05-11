using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Videos.DeleteVideos;

public record DeleteVideosResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<string> Data
);
