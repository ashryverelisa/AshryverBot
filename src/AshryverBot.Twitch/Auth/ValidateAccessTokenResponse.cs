using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Auth;

public record ValidateAccessTokenResponse
(
    [property: JsonPropertyName("client_id")] string ClientId,
    [property: JsonPropertyName("login")] string Login,
    [property: JsonPropertyName("scopes")] IReadOnlyCollection<string> Scopes,
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("expires_in")] int ExpiresIn
);