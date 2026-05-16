using AshryverBot.Database.Entities;

namespace AshryverBot.Database.Repositories.Interfaces;

public interface IWatchtimeRepository
{
    Task<WatchtimeEntity?> GetByUserIdAsync(string twitchUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds <paramref name="seconds"/> to each chatter in <paramref name="chatters"/>, creating rows
    /// for users that don't have one yet. Returns the number of rows that were touched.
    /// </summary>
    Task<int> AddSecondsAsync(
        IReadOnlyCollection<WatchtimeChatter> chatters,
        long seconds,
        DateTimeOffset now,
        CancellationToken cancellationToken = default);
}

public record WatchtimeChatter(string TwitchUserId, string Login, string? DisplayName);
