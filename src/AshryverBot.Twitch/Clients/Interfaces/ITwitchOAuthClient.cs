using AshryverBot.Twitch.Auth;

namespace AshryverBot.Twitch.Clients.Interfaces;

public interface ITwitchOAuthClient
{
    Task<RefreshResponse> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<bool> RevokeAsync(string accessToken, CancellationToken cancellationToken = default);
}
