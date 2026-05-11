using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Chat.SendChatMessage;

public record SendChatMessageRequest
{
    [JsonPropertyName("broadcaster_id")]
    public string BroadcasterId { get; init; } = string.Empty;

    [JsonPropertyName("sender_id")]
    public string SenderId { get; init; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; init; } = string.Empty;

    [JsonPropertyName("reply_parent_message_id")]
    public string? ReplyParentMessageId { get; init; }

    [JsonPropertyName("for_source_only")]
    public bool? ForSourceOnly { get; init; }
}
