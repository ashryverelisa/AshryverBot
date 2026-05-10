using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Moderation.Common;

public record BannedUser(
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("user_login")] string UserLogin,
    [property: JsonPropertyName("user_name")] string UserName,
    [property: JsonPropertyName("expires_at")] DateTimeOffset? ExpiresAt,
    [property: JsonPropertyName("created_at")] DateTimeOffset CreatedAt,
    [property: JsonPropertyName("reason")] string Reason,
    [property: JsonPropertyName("moderator_id")] string ModeratorId,
    [property: JsonPropertyName("moderator_login")] string ModeratorLogin,
    [property: JsonPropertyName("moderator_name")] string ModeratorName
);
