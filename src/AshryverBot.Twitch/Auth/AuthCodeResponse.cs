using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Auth;

public record AuthCodeResponse(
    [property: JsonPropertyName("access_token")] string AccessToken,
    [property: JsonPropertyName("refresh_token")] string RefreshToken,
    [property: JsonPropertyName("expires_in")] int ExpiresIn,
    [property: JsonPropertyName("scope")] IReadOnlyCollection<string> Scopes,
    [property: JsonPropertyName("token_type")] string TokenType
);