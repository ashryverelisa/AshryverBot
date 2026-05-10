using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Teams.Common;

public record Team(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("team_name")] string TeamName,
    [property: JsonPropertyName("team_display_name")] string TeamDisplayName,
    [property: JsonPropertyName("background_image_url")] string? BackgroundImageUrl,
    [property: JsonPropertyName("banner")] string? Banner,
    [property: JsonPropertyName("created_at")] DateTimeOffset CreatedAt,
    [property: JsonPropertyName("updated_at")] DateTimeOffset UpdatedAt,
    [property: JsonPropertyName("info")] string Info,
    [property: JsonPropertyName("thumbnail_url")] string ThumbnailUrl,
    [property: JsonPropertyName("users")] IReadOnlyCollection<TeamMember>? Users
);

public record TeamMember(
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("user_login")] string UserLogin,
    [property: JsonPropertyName("user_name")] string UserName
);
