using System.ComponentModel.DataAnnotations;

namespace AshryverBot.Database.Entities;

public class CommandEntity
{
    public Guid Id { get; set; }

    [MaxLength(64)]
    public required string Name { get; set; }

    [MaxLength(500)]
    public required string Response { get; set; }

    public bool IsEnabled { get; set; } = true;

    public int CooldownSeconds { get; set; }

    public CommandRole RequiredRole { get; set; } = CommandRole.Everyone;

    public long UsageCount { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
