using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.Streams.CreateStreamMarker;
using AshryverBot.Twitch.Helix.Models.Streams.GetFollowedStreams;
using AshryverBot.Twitch.Helix.Models.Streams.GetStreamKey;
using AshryverBot.Twitch.Helix.Models.Streams.GetStreamMarkers;
using AshryverBot.Twitch.Helix.Models.Streams.GetStreams;

namespace AshryverBot.Twitch.Helix.Apis;

public class StreamsApi(ITwitchClient client) : IStreamsApi
{
    public Task<GetStreamKeyResponse> GetStreamKeyAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        return client.GetAsync<GetStreamKeyResponse>("streams/key", accessToken, query, cancellationToken);
    }

    public Task<GetStreamsResponse> GetStreamsAsync(
        string accessToken,
        IEnumerable<string>? userIds = null,
        IEnumerable<string>? userLogins = null,
        IEnumerable<string>? gameIds = null,
        string? type = null,
        IEnumerable<string>? languages = null,
        int? first = null,
        string? before = null,
        string? after = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddMany("user_id", userIds);
        query.AddMany("user_login", userLogins);
        query.AddMany("game_id", gameIds);
        query.AddIfNotNull("type", type);
        query.AddMany("language", languages);
        query.AddIfNotNull("first", first);
        query.AddIfNotNull("before", before);
        query.AddIfNotNull("after", after);
        return client.GetAsync<GetStreamsResponse>("streams", accessToken, query, cancellationToken);
    }

    public Task<GetFollowedStreamsResponse> GetFollowedStreamsAsync(
        string accessToken,
        string userId,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("user_id", userId),
        };
        query.AddIfNotNull("first", first);
        query.AddIfNotNull("after", after);
        return client.GetAsync<GetFollowedStreamsResponse>("streams/followed", accessToken, query, cancellationToken);
    }

    public Task<CreateStreamMarkerResponse> CreateStreamMarkerAsync(
        string accessToken,
        CreateStreamMarkerRequest body,
        CancellationToken cancellationToken = default)
        => client.PostAsync<CreateStreamMarkerRequest, CreateStreamMarkerResponse>("streams/markers", accessToken, body, queryParameters: null, cancellationToken);

    public Task<GetStreamMarkersResponse> GetStreamMarkersAsync(
        string accessToken,
        string? userId = null,
        string? videoId = null,
        string? after = null,
        string? before = null,
        int? first = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddIfNotNull("user_id", userId);
        query.AddIfNotNull("video_id", videoId);
        query.AddIfNotNull("after", after);
        query.AddIfNotNull("before", before);
        query.AddIfNotNull("first", first);
        return client.GetAsync<GetStreamMarkersResponse>("streams/markers", accessToken, query, cancellationToken);
    }
}
