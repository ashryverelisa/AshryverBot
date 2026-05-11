using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.Analytics.GetExtensionAnalytics;
using AshryverBot.Twitch.Helix.Models.Analytics.GetGameAnalytics;

namespace AshryverBot.Twitch.Helix.Apis;

public class AnalyticsApi(ITwitchClient client) : IAnalyticsApi
{
    public Task<GetExtensionAnalyticsResponse> GetExtensionAnalyticsAsync(
        string accessToken,
        string? extensionId = null,
        string? type = null,
        DateTimeOffset? startedAt = null,
        DateTimeOffset? endedAt = null,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddIfNotNull("extension_id", extensionId);
        query.AddIfNotNull("type", type);
        query.AddIfNotNull("started_at", startedAt);
        query.AddIfNotNull("ended_at", endedAt);
        query.AddIfNotNull("first", first);
        query.AddIfNotNull("after", after);
        return client.GetAsync<GetExtensionAnalyticsResponse>("analytics/extensions", accessToken, query, cancellationToken);
    }

    public Task<GetGameAnalyticsResponse> GetGameAnalyticsAsync(
        string accessToken,
        string? gameId = null,
        string? type = null,
        DateTimeOffset? startedAt = null,
        DateTimeOffset? endedAt = null,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddIfNotNull("game_id", gameId);
        query.AddIfNotNull("type", type);
        query.AddIfNotNull("started_at", startedAt);
        query.AddIfNotNull("ended_at", endedAt);
        query.AddIfNotNull("first", first);
        query.AddIfNotNull("after", after);
        return client.GetAsync<GetGameAnalyticsResponse>("analytics/games", accessToken, query, cancellationToken);
    }
}
