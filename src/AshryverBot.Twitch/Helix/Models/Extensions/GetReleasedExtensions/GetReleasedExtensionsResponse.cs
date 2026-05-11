using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Extensions.Common;

namespace AshryverBot.Twitch.Helix.Models.Extensions.GetReleasedExtensions;

public record GetReleasedExtensionsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<Extension> Data
);
