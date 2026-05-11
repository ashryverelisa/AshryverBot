using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Extensions.GetExtensionConfigurationSegment;

public record GetExtensionConfigurationSegmentResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ExtensionConfigurationSegment> Data
);
