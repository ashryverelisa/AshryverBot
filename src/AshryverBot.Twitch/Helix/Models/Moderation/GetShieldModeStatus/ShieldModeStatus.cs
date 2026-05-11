using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Moderation.GetShieldModeStatus;

public record ShieldModeStatus(
    [property: JsonPropertyName("is_active")] bool IsActive,
    [property: JsonPropertyName("moderator_id")] string ModeratorId,
    [property: JsonPropertyName("moderator_login")] string ModeratorLogin,
    [property: JsonPropertyName("moderator_name")] string ModeratorName,
    [property: JsonPropertyName("last_activated_at")] DateTimeOffset? LastActivatedAt
);
