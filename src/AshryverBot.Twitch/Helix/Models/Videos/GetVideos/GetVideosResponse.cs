using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Videos.GetVideos;

public record GetVideosResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<Video> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
