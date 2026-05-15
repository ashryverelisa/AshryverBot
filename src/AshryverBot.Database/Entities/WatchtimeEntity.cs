using System.ComponentModel.DataAnnotations;

namespace AshryverBot.Database.Entities;

public class WatchtimeEntity
{
    public Guid Id { get; set; }

    [MaxLength(64)]
    public required string TwitchUserId { get; set; }

    [MaxLength(64)]
    public required string Login { get; set; }

    [MaxLength(128)]
    public string? DisplayName { get; set; }

    public long TotalSeconds { get; set; }

    public DateTimeOffset LastSeenAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
