using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Moderation.ManageHeldAutoModMessages;

public record ManageHeldAutoModMessagesRequest(
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("msg_id")] string MsgId,
    [property: JsonPropertyName("action")] string Action
);
