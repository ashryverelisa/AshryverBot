using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.Videos.DeleteVideos;
using AshryverBot.Twitch.Helix.Models.Videos.GetVideos;

namespace AshryverBot.Twitch.Helix.Apis;

public class VideosApi(ITwitchClient client) : IVideosApi
{
    public Task<GetVideosResponse> GetVideosAsync(
        string accessToken,
        IEnumerable<string>? ids = null,
        string? userId = null,
        string? gameId = null,
        string? language = null,
        string? period = null,
        string? sort = null,
        string? type = null,
        int? first = null,
        string? after = null,
        string? before = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddMany("id", ids);
        query.AddIfNotNull("user_id", userId);
        query.AddIfNotNull("game_id", gameId);
        query.AddIfNotNull("language", language);
        query.AddIfNotNull("period", period);
        query.AddIfNotNull("sort", sort);
        query.AddIfNotNull("type", type);
        query.AddIfNotNull("first", first);
        query.AddIfNotNull("after", after);
        query.AddIfNotNull("before", before);
        return client.GetAsync<GetVideosResponse>("videos", accessToken, query, cancellationToken);
    }

    public Task<DeleteVideosResponse> DeleteVideosAsync(
        string accessToken,
        IEnumerable<string> ids,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddMany("id", ids);
        return client.DeleteAsync<DeleteVideosResponse>("videos", accessToken, query, cancellationToken);
    }
}
