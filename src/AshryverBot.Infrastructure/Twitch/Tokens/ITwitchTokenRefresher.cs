namespace AshryverBot.Infrastructure.Twitch.Tokens;

public interface ITwitchTokenRefresher
{
    /// <summary>
    /// Returns a non-expired access token for the given user, refreshing it if it expires within the safety window.
    /// </summary>
    Task<TwitchTokenInfo?> GetValidAsync(string twitchUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Forces a refresh and persists the new token.
    /// </summary>
    Task<TwitchTokenInfo> RefreshAsync(TwitchTokenInfo current, CancellationToken cancellationToken = default);
}
