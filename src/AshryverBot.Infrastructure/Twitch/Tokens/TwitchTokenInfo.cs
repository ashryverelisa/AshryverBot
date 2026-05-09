namespace AshryverBot.Infrastructure.Twitch.Tokens;

public record TwitchTokenInfo(
    string TwitchUserId,
    string Login,
    string? DisplayName,
    string? Email,
    string AccessToken,
    string RefreshToken,
    DateTimeOffset ExpiresAt,
    IReadOnlyCollection<string> Scopes,
    bool IsBotAccount);
