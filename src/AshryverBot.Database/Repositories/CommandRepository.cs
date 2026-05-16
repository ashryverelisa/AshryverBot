using AshryverBot.Database.Entities;
using AshryverBot.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AshryverBot.Database.Repositories;

public class CommandRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    : ICommandRepository
{
    public async Task<CommandEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await db.Commands
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<CommandEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await db.Commands
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public async Task<IReadOnlyList<CommandEntity>> ListAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await db.Commands
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task CreateAsync(CommandEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        db.Commands.Add(entity);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(CommandEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        db.Commands.Update(entity);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        await db.Commands
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
