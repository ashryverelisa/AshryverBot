using AshryverBot.Database.Entities;

namespace AshryverBot.Infrastructure.Commands.Interfaces;

public interface ICommandService
{
    Task<IReadOnlyList<CommandEntity>> ListAsync(CancellationToken cancellationToken = default);
    Task<CommandEntity> CreateAsync(CommandDraft draft, CancellationToken cancellationToken = default);
    Task<CommandEntity> UpdateAsync(Guid id, CommandDraft draft, CancellationToken cancellationToken = default);
    Task<CommandEntity> SetEnabledAsync(Guid id, bool enabled, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
