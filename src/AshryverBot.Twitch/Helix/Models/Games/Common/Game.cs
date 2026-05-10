using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Games.Common;

public record Game(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("box_art_url")] string BoxArtUrl,
    [property: JsonPropertyName("igdb_id")] string IgdbId
);
