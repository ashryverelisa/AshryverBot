using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Extensions.SetExtensionRequiredConfiguration;

public record SetExtensionRequiredConfigurationRequest(
    [property: JsonPropertyName("extension_id")] string ExtensionId,
    [property: JsonPropertyName("extension_version")] string ExtensionVersion,
    [property: JsonPropertyName("required_configuration")] string RequiredConfiguration
);
