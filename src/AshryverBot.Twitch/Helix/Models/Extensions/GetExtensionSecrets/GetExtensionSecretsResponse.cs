using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Extensions.GetExtensionSecrets;

public record GetExtensionSecretsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ExtensionSecretSet> Data
);
