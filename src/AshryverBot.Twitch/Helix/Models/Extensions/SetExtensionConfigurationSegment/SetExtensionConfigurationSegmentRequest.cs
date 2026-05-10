using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Extensions.SetExtensionConfigurationSegment;

public record SetExtensionConfigurationSegmentRequest(
    [property: JsonPropertyName("extension_id")] string ExtensionId,
    [property: JsonPropertyName("segment")] string Segment,
    [property: JsonPropertyName("broadcaster_id")] string? BroadcasterId,
    [property: JsonPropertyName("content")] string? Content,
    [property: JsonPropertyName("version")] string? Version
);
