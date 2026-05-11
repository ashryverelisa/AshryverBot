using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.HypeTrain.GetHypeTrainEvents;

namespace AshryverBot.Twitch.Helix.Apis;

public class HypeTrainApi(ITwitchClient client) : IHypeTrainApi
{
    public Task<GetHypeTrainEventsResponse> GetHypeTrainEventsAsync(
        string accessToken,
        string broadcasterId,
        int? first = null,
        string? id = null,
        string? after = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        query.AddIfNotNull("first", first);
        query.AddIfNotNull("id", id);
        query.AddIfNotNull("after", after);
        return client.GetAsync<GetHypeTrainEventsResponse>("hypetrain/events", accessToken, query, cancellationToken);
    }
}
