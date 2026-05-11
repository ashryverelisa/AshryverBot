using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Moderation.WarnChatUser;

public record WarnChatUserResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<WarnChatUserResult> Data
);

public record WarnChatUserResult(
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("moderator_id")] string ModeratorId,
    [property: JsonPropertyName("reason")] string Reason
);
