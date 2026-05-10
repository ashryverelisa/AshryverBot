using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Extensions.SendExtensionPubSubMessage;

public record SendExtensionPubSubMessageRequest(
    [property: JsonPropertyName("target")] IReadOnlyCollection<string> Target,
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("is_global_broadcast")] bool IsGlobalBroadcast,
    [property: JsonPropertyName("message")] string Message
);
