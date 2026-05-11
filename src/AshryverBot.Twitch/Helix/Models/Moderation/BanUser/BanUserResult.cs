using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Moderation.BanUser;

public record BanUserResult(
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("moderator_id")] string ModeratorId,
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("created_at")] DateTimeOffset CreatedAt,
    [property: JsonPropertyName("end_time")] DateTimeOffset? EndTime
);
