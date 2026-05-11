using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.Teams.GetChannelTeams;
using AshryverBot.Twitch.Helix.Models.Teams.GetTeams;

namespace AshryverBot.Twitch.Helix.Apis;

public class TeamsApi(ITwitchClient client) : ITeamsApi
{
    public Task<GetChannelTeamsResponse> GetChannelTeamsAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        return client.GetAsync<GetChannelTeamsResponse>("teams/channel", accessToken, query, cancellationToken);
    }

    public Task<GetTeamsResponse> GetTeamsAsync(
        string accessToken,
        string? name = null,
        string? id = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddIfNotNull("name", name);
        query.AddIfNotNull("id", id);
        return client.GetAsync<GetTeamsResponse>("teams", accessToken, query, cancellationToken);
    }
}
