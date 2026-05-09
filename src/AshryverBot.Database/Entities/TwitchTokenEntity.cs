namespace AshryverBot.Database.Entities;

public class TwitchTokenEntity
{
    public string TwitchUserId { get; set; } = null!;
    public string Login { get; set; } = null!;
    public string? DisplayName { get; set; }
    public string? Email { get; set; }
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTimeOffset ExpiresAt { get; set; }
    public string Scopes { get; set; } = string.Empty;
    public bool IsBotAccount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
