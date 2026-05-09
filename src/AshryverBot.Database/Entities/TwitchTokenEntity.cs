using System.ComponentModel.DataAnnotations;

namespace AshryverBot.Database.Entities;

public class TwitchTokenEntity
{
    [MaxLength(64)]
    public required string TwitchUserId { get; set; }

    [MaxLength(64)]
    public required string Login { get; set; }

    [MaxLength(128)]
    public string? DisplayName { get; set; }

    [MaxLength(256)]
    public string? Email { get; set; }

    [MaxLength(512)]
    public required string AccessToken { get; set; }

    [MaxLength(512)]
    public required string RefreshToken { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }

    [MaxLength(4096)]
    public string Scopes { get; set; } = string.Empty;

    public bool IsBotAccount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
