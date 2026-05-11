using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Chat.SendChatAnnouncement;

public record SendChatAnnouncementRequest(
    [property: JsonPropertyName("message")] string Message,
    [property: JsonPropertyName("color")] string? Color
);
