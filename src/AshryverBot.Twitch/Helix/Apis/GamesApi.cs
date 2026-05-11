using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.Games.GetGames;
using AshryverBot.Twitch.Helix.Models.Games.GetTopGames;

namespace AshryverBot.Twitch.Helix.Apis;

public class GamesApi(ITwitchClient client) : IGamesApi
{
    public Task<GetTopGamesResponse> GetTopGamesAsync(
        string accessToken,
        int? first = null,
        string? after = null,
        string? before = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddIfNotNull("first", first);
        query.AddIfNotNull("after", after);
        query.AddIfNotNull("before", before);
        return client.GetAsync<GetTopGamesResponse>("games/top", accessToken, query, cancellationToken);
    }

    public Task<GetGamesResponse> GetGamesAsync(
        string accessToken,
        IEnumerable<string>? ids = null,
        IEnumerable<string>? names = null,
        IEnumerable<string>? igdbIds = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddMany("id", ids);
        query.AddMany("name", names);
        query.AddMany("igdb_id", igdbIds);
        return client.GetAsync<GetGamesResponse>("games", accessToken, query, cancellationToken);
    }
}
