using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Extensions.SendExtensionChatMessage;

public record SendExtensionChatMessageRequest(
    [property: JsonPropertyName("text")] string Text,
    [property: JsonPropertyName("extension_id")] string ExtensionId,
    [property: JsonPropertyName("extension_version")] string ExtensionVersion
);
