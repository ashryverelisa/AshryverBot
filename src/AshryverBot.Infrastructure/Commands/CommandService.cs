using AshryverBot.Database.Entities;
using AshryverBot.Database.Repositories.Interfaces;
using AshryverBot.Infrastructure.Commands.Interfaces;

namespace AshryverBot.Infrastructure.Commands;

public class CommandService(
    ICommandRepository repository,
    TimeProvider timeProvider) : ICommandService
{
    public Task<IReadOnlyList<CommandEntity>> ListAsync(CancellationToken cancellationToken = default)
        => repository.ListAsync(cancellationToken);

    public async Task<CommandEntity> CreateAsync(CommandDraft draft, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(draft);

        var now = timeProvider.GetUtcNow();
        var entity = new CommandEntity
        {
            Id = Guid.NewGuid(),
            Name = draft.Name,
            Response = draft.Response,
            CooldownSeconds = draft.CooldownSeconds,
            RequiredRole = draft.RequiredRole,
            IsEnabled = draft.IsEnabled,
            UsageCount = 0,
            CreatedAt = now,
            UpdatedAt = now,
        };

        await repository.CreateAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<CommandEntity> UpdateAsync(Guid id, CommandDraft draft, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(draft);

        var entity = await repository.GetAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"Command {id} not found.");

        entity.Name = draft.Name;
        entity.Response = draft.Response;
        entity.CooldownSeconds = draft.CooldownSeconds;
        entity.RequiredRole = draft.RequiredRole;
        entity.IsEnabled = draft.IsEnabled;
        entity.UpdatedAt = timeProvider.GetUtcNow();

        await repository.UpdateAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<CommandEntity> SetEnabledAsync(Guid id, bool enabled, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"Command {id} not found.");

        entity.IsEnabled = enabled;
        entity.UpdatedAt = timeProvider.GetUtcNow();

        await repository.UpdateAsync(entity, cancellationToken);
        return entity;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        => repository.DeleteAsync(id, cancellationToken);
}
