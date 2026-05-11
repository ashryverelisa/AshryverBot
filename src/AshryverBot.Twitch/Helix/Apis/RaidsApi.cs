using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Models.Raids.StartARaid;

namespace AshryverBot.Twitch.Helix.Apis;

public class RaidsApi(ITwitchClient client) : IRaidsApi
{
    public Task<StartARaidResponse> StartARaidAsync(
        string accessToken,
        string fromBroadcasterId,
        string toBroadcasterId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("from_broadcaster_id", fromBroadcasterId),
            new("to_broadcaster_id", toBroadcasterId),
        };
        return client.PostAsync<StartARaidResponse>("raids", accessToken, query, cancellationToken);
    }

    public Task CancelARaidAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        return client.DeleteAsync("raids", accessToken, query, cancellationToken);
    }
}
