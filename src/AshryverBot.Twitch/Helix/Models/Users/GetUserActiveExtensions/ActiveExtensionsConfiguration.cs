using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Users.GetUserActiveExtensions;

public record ActiveExtensionsConfiguration(
    [property: JsonPropertyName("panel")] IReadOnlyDictionary<string, ActiveExtensionSlot> Panel,
    [property: JsonPropertyName("overlay")] IReadOnlyDictionary<string, ActiveExtensionSlot> Overlay,
    [property: JsonPropertyName("component")] IReadOnlyDictionary<string, ActiveExtensionSlot> Component
);

public record ActiveExtensionSlot(
    [property: JsonPropertyName("active")] bool Active,
    [property: JsonPropertyName("id")] string? Id,
    [property: JsonPropertyName("version")] string? Version,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("x")] int? X,
    [property: JsonPropertyName("y")] int? Y
);
