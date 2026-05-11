using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Search.Common;

public record SearchedCategory(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("box_art_url")] string BoxArtUrl
);
