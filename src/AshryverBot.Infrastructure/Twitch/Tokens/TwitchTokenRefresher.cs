using AshryverBot.Database.Repositories;
using AshryverBot.Database.Repositories.Interfaces;
using AshryverBot.Twitch.Clients.Interfaces;
using Microsoft.Extensions.Logging;

namespace AshryverBot.Infrastructure.Twitch.Tokens;

public class TwitchTokenRefresher(
    ITwitchTokenRepository repository,
    ITwitchOAuthClient oauthClient,
    TimeProvider timeProvider,
    ILogger<TwitchTokenRefresher> logger) : ITwitchTokenRefresher
{
    private static readonly TimeSpan _refreshSafetyWindow = TimeSpan.FromMinutes(5);

    public async Task<TwitchTokenInfo?> GetValidAsync(string twitchUserId, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(twitchUserId, cancellationToken);

        if (entity is null)
            return null;

        var current = entity.ToInfo();
        if (current.ExpiresAt - timeProvider.GetUtcNow() > _refreshSafetyWindow)
            return current;

        try
        {
            return await RefreshAsync(current, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to refresh Twitch token for user {TwitchUserId}", twitchUserId);
            return null;
        }
    }

    public async Task<TwitchTokenInfo> RefreshAsync(TwitchTokenInfo current, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(current);

        var refreshed = await oauthClient.RefreshAsync(current.RefreshToken, cancellationToken);
        var expiresAt = timeProvider.GetUtcNow().AddSeconds(refreshed.ExpiresIn);

        var updated = current with
        {
            AccessToken = refreshed.AccessToken,
            RefreshToken = refreshed.RefreshToken,
            ExpiresAt = expiresAt,
            Scopes = refreshed.Scopes,
        };

        await repository.UpsertAsync(updated, timeProvider.GetUtcNow(), cancellationToken);

        logger.LogInformation("Refreshed Twitch token for user {TwitchUserId}, valid until {ExpiresAt}",
            updated.TwitchUserId, updated.ExpiresAt);

        return updated;
    }
}
