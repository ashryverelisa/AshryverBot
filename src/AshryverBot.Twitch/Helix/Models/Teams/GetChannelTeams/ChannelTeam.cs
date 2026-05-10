using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Teams.GetChannelTeams;

public record ChannelTeam(
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("broadcaster_login")] string BroadcasterLogin,
    [property: JsonPropertyName("broadcaster_name")] string BroadcasterName,
    [property: JsonPropertyName("background_image_url")] string? BackgroundImageUrl,
    [property: JsonPropertyName("banner")] string? Banner,
    [property: JsonPropertyName("created_at")] DateTimeOffset CreatedAt,
    [property: JsonPropertyName("updated_at")] DateTimeOffset UpdatedAt,
    [property: JsonPropertyName("info")] string Info,
    [property: JsonPropertyName("thumbnail_url")] string ThumbnailUrl,
    [property: JsonPropertyName("team_name")] string TeamName,
    [property: JsonPropertyName("team_display_name")] string TeamDisplayName,
    [property: JsonPropertyName("id")] string Id
);
