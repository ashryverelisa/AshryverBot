using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Chat.GetChatSettings;

namespace AshryverBot.Twitch.Helix.Models.Chat.UpdateChatSettings;

public record UpdateChatSettingsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ChatSettings> Data
);
