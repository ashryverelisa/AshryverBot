using AshryverBot.Database.Entities;
using AshryverBot.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AshryverBot.Database.Repositories;

public class TwitchTokenRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    : ITwitchTokenRepository
{
    public async Task<TwitchTokenEntity?> GetAsync(string twitchUserId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(twitchUserId);
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await db.TwitchTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TwitchUserId == twitchUserId, cancellationToken);
    }

    public async Task<TwitchTokenEntity?> GetBotAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await db.TwitchTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IsBotAccount, cancellationToken);
    }

    public async Task CreateAsync(TwitchTokenEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        db.TwitchTokens.Add(entity);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TwitchTokenEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        db.TwitchTokens.Update(entity);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string twitchUserId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(twitchUserId);
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        await db.TwitchTokens
            .Where(x => x.TwitchUserId == twitchUserId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
