using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.Bits.GetBitsLeaderboard;
using AshryverBot.Twitch.Helix.Models.Bits.GetCheermotes;
using AshryverBot.Twitch.Helix.Models.Bits.GetExtensionTransactions;

namespace AshryverBot.Twitch.Helix.Apis;

public class BitsApi(ITwitchClient client) : IBitsApi
{
    public Task<GetBitsLeaderboardResponse> GetBitsLeaderboardAsync(
        string accessToken,
        int? count = null,
        string? period = null,
        DateTimeOffset? startedAt = null,
        string? userId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddIfNotNull("count", count);
        query.AddIfNotNull("period", period);
        query.AddIfNotNull("started_at", startedAt);
        query.AddIfNotNull("user_id", userId);
        return client.GetAsync<GetBitsLeaderboardResponse>("bits/leaderboard", accessToken, query, cancellationToken);
    }

    public Task<GetCheermotesResponse> GetCheermotesAsync(
        string accessToken,
        string? broadcasterId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddIfNotNull("broadcaster_id", broadcasterId);
        return client.GetAsync<GetCheermotesResponse>("bits/cheermotes", accessToken, query, cancellationToken);
    }

    public Task<GetExtensionTransactionsResponse> GetExtensionTransactionsAsync(
        string accessToken,
        string extensionId,
        IEnumerable<string>? ids = null,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("extension_id", extensionId),
        };
        query.AddMany("id", ids);
        query.AddIfNotNull("first", first);
        query.AddIfNotNull("after", after);
        return client.GetAsync<GetExtensionTransactionsResponse>("extensions/transactions", accessToken, query, cancellationToken);
    }
}
