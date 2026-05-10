using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Chat.SendChatMessage;

public record SendChatMessageResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<SendChatMessageResult> Data
);
