using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Users.GetUserExtensions;

public record UserExtension(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("version")] string Version,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("can_activate")] bool CanActivate,
    [property: JsonPropertyName("type")] IReadOnlyCollection<string> Type
);
