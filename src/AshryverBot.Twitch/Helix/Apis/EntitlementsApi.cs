using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.Entitlements.GetDropsEntitlements;
using AshryverBot.Twitch.Helix.Models.Entitlements.UpdateDropsEntitlements;

namespace AshryverBot.Twitch.Helix.Apis;

public class EntitlementsApi(ITwitchClient client) : IEntitlementsApi
{
    public Task<GetDropsEntitlementsResponse> GetDropsEntitlementsAsync(
        string accessToken,
        IEnumerable<string>? ids = null,
        string? userId = null,
        string? gameId = null,
        string? fulfillmentStatus = null,
        string? after = null,
        int? first = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddMany("id", ids);
        query.AddIfNotNull("user_id", userId);
        query.AddIfNotNull("game_id", gameId);
        query.AddIfNotNull("fulfillment_status", fulfillmentStatus);
        query.AddIfNotNull("after", after);
        query.AddIfNotNull("first", first);
        return client.GetAsync<GetDropsEntitlementsResponse>("entitlements/drops", accessToken, query, cancellationToken);
    }

    public Task<UpdateDropsEntitlementsResponse> UpdateDropsEntitlementsAsync(
        string accessToken,
        UpdateDropsEntitlementsRequest body,
        CancellationToken cancellationToken = default)
        => client.PatchAsync<UpdateDropsEntitlementsRequest, UpdateDropsEntitlementsResponse>(
            "entitlements/drops", accessToken, body, queryParameters: null, cancellationToken);
}
