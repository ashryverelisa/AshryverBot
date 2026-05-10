using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Extensions.GetExtensionSecrets;

namespace AshryverBot.Twitch.Helix.Models.Extensions.CreateExtensionSecret;

public record CreateExtensionSecretResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ExtensionSecretSet> Data
);
