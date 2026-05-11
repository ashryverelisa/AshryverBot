using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Moderation.GetUnbanRequests;

public record UnbanRequest(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("broadcaster_name")] string BroadcasterName,
    [property: JsonPropertyName("broadcaster_login")] string BroadcasterLogin,
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("moderator_id")] string? ModeratorId,
    [property: JsonPropertyName("moderator_login")] string? ModeratorLogin,
    [property: JsonPropertyName("moderator_name")] string? ModeratorName,
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("user_login")] string UserLogin,
    [property: JsonPropertyName("user_name")] string UserName,
    [property: JsonPropertyName("text")] string Text,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("created_at")] DateTimeOffset CreatedAt,
    [property: JsonPropertyName("resolved_at")] DateTimeOffset? ResolvedAt,
    [property: JsonPropertyName("resolution_text")] string? ResolutionText
);
