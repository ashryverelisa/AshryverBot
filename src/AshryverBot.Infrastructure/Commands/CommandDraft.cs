using AshryverBot.Database.Entities;

namespace AshryverBot.Infrastructure.Commands;

public record CommandDraft(
    string Name,
    string Response,
    int CooldownSeconds,
    CommandRole RequiredRole,
    bool IsEnabled);
