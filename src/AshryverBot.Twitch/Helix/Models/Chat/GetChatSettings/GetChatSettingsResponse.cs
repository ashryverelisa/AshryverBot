using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Chat.GetChatSettings;

public record GetChatSettingsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ChatSettings> Data
);
