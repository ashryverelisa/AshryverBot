using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.Clips.CreateClip;
using AshryverBot.Twitch.Helix.Models.Clips.GetClips;

namespace AshryverBot.Twitch.Helix.Apis;

public class ClipsApi(ITwitchClient client) : IClipsApi
{
    public Task<CreateClipResponse> CreateClipAsync(
        string accessToken,
        string broadcasterId,
        bool? hasDelay = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        query.AddIfNotNull("has_delay", hasDelay);
        return client.PostAsync<CreateClipResponse>("clips", accessToken, query, cancellationToken);
    }

    public Task<GetClipsResponse> GetClipsAsync(
        string accessToken,
        string? broadcasterId = null,
        string? gameId = null,
        IEnumerable<string>? ids = null,
        DateTimeOffset? startedAt = null,
        DateTimeOffset? endedAt = null,
        int? first = null,
        string? before = null,
        string? after = null,
        bool? isFeatured = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddIfNotNull("broadcaster_id", broadcasterId);
        query.AddIfNotNull("game_id", gameId);
        query.AddMany("id", ids);
        query.AddIfNotNull("started_at", startedAt);
        query.AddIfNotNull("ended_at", endedAt);
        query.AddIfNotNull("first", first);
        query.AddIfNotNull("before", before);
        query.AddIfNotNull("after", after);
        query.AddIfNotNull("is_featured", isFeatured);
        return client.GetAsync<GetClipsResponse>("clips", accessToken, query, cancellationToken);
    }
}
