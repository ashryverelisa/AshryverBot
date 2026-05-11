using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Moderation.WarnChatUser;

public record WarnChatUserRequest(
    [property: JsonPropertyName("data")] WarnChatUserPayload Data
);

public record WarnChatUserPayload(
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("reason")] string Reason
);
