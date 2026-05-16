using AshryverBot.Database.Entities;

namespace AshryverBot.Database.Repositories.Interfaces;

public interface ITwitchTokenRepository
{
    Task<TwitchTokenEntity?> GetAsync(string twitchUserId, CancellationToken cancellationToken = default);
    Task<TwitchTokenEntity?> GetBotAsync(CancellationToken cancellationToken = default);
    Task CreateAsync(TwitchTokenEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TwitchTokenEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(string twitchUserId, CancellationToken cancellationToken = default);
}
