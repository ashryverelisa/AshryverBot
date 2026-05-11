using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Users.GetUserActiveExtensions;

namespace AshryverBot.Twitch.Helix.Models.Users.UpdateUserExtensions;

public record UpdateUserExtensionsRequest(
    [property: JsonPropertyName("data")] ActiveExtensionsConfiguration Data
);
