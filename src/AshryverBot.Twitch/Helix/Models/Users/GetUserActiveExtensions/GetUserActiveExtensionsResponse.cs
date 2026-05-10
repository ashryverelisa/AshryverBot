using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Users.GetUserActiveExtensions;

public record GetUserActiveExtensionsResponse(
    [property: JsonPropertyName("data")] ActiveExtensionsConfiguration Data
);
