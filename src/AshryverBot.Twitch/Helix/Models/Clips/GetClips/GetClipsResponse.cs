using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Clips.GetClips;

public record GetClipsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<Clip> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
