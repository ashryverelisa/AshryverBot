using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Users.GetUserExtensions;

public record GetUserExtensionsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<UserExtension> Data
);
