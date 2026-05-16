using AshryverBot.Database.Entities;
using AshryverBot.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AshryverBot.Database.Repositories;

public class WatchtimeRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    : IWatchtimeRepository
{
    public async Task<WatchtimeEntity?> GetByUserIdAsync(string twitchUserId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(twitchUserId);
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await db.Watchtimes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TwitchUserId == twitchUserId, cancellationToken);
    }

    public async Task<int> AddSecondsAsync(
        IReadOnlyCollection<WatchtimeChatter> chatters,
        long seconds,
        DateTimeOffset now,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(chatters);
        if (chatters.Count == 0 || seconds <= 0) return 0;

        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var userIds = chatters.Select(c => c.TwitchUserId).ToArray();
        var existing = await db.Watchtimes
            .Where(x => userIds.Contains(x.TwitchUserId))
            .ToDictionaryAsync(x => x.TwitchUserId, cancellationToken);

        foreach (var chatter in chatters)
        {
            if (existing.TryGetValue(chatter.TwitchUserId, out var row))
            {
                row.TotalSeconds += seconds;
                row.LastSeenAt = now;
                row.UpdatedAt = now;
                row.Login = chatter.Login;
                row.DisplayName = chatter.DisplayName;
            }
            else
            {
                db.Watchtimes.Add(new WatchtimeEntity
                {
                    TwitchUserId = chatter.TwitchUserId,
                    Login = chatter.Login,
                    DisplayName = chatter.DisplayName,
                    TotalSeconds = seconds,
                    LastSeenAt = now,
                    CreatedAt = now,
                    UpdatedAt = now,
                });
            }
        }

        return await db.SaveChangesAsync(cancellationToken);
    }
}
