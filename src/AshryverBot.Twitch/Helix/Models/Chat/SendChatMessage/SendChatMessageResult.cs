using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Chat.SendChatMessage;

public record SendChatMessageResult(
    [property: JsonPropertyName("message_id")] string MessageId,
    [property: JsonPropertyName("is_sent")] bool IsSent,
    [property: JsonPropertyName("drop_reason")] DropReason? DropReason
);

public record DropReason(
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("message")] string Message
);
