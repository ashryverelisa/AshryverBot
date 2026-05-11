using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Extensions.GetExtensionConfigurationSegment;

public record ExtensionConfigurationSegment(
    [property: JsonPropertyName("segment")] string Segment,
    [property: JsonPropertyName("broadcaster_id")] string? BroadcasterId,
    [property: JsonPropertyName("content")] string Content,
    [property: JsonPropertyName("version")] string Version
);
