using AshryverBot.Twitch.Helix.Models.Raids.StartARaid;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IRaidsApi
{
    Task<StartARaidResponse> StartARaidAsync(
        string accessToken,
        string fromBroadcasterId,
        string toBroadcasterId,
        CancellationToken cancellationToken = default);

    Task CancelARaidAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default);
}
