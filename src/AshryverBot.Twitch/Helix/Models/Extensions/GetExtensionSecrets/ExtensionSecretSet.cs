using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Extensions.GetExtensionSecrets;

public record ExtensionSecretSet(
    [property: JsonPropertyName("format_version")] int FormatVersion,
    [property: JsonPropertyName("secrets")] IReadOnlyCollection<ExtensionSecret> Secrets
);

public record ExtensionSecret(
    [property: JsonPropertyName("content")] string Content,
    [property: JsonPropertyName("active_at")] DateTimeOffset ActiveAt,
    [property: JsonPropertyName("expires_at")] DateTimeOffset ExpiresAt
);
