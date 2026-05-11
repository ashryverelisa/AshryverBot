using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Extensions.Common;

namespace AshryverBot.Twitch.Helix.Models.Extensions.GetExtensions;

public record GetExtensionsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<Extension> Data
);
