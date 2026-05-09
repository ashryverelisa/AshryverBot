using AshryverBot.Database.Entities;

namespace AshryverBot.Database.Repositories;

public interface ICommandRepository
{
    Task<CommandEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CommandEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CommandEntity>> ListAsync(CancellationToken cancellationToken = default);
    Task CreateAsync(CommandEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(CommandEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
