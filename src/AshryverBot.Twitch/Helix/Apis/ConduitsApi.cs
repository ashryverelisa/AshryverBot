using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.Conduits.CreateConduits;
using AshryverBot.Twitch.Helix.Models.Conduits.GetConduits;
using AshryverBot.Twitch.Helix.Models.Conduits.GetConduitShards;
using AshryverBot.Twitch.Helix.Models.Conduits.UpdateConduits;
using AshryverBot.Twitch.Helix.Models.Conduits.UpdateConduitShards;

namespace AshryverBot.Twitch.Helix.Apis;

public class ConduitsApi(ITwitchClient client) : IConduitsApi
{
    public Task<GetConduitsResponse> GetConduitsAsync(
        string accessToken,
        CancellationToken cancellationToken = default)
        => client.GetAsync<GetConduitsResponse>("eventsub/conduits", accessToken, queryParameters: null, cancellationToken);

    public Task<CreateConduitsResponse> CreateConduitsAsync(
        string accessToken,
        CreateConduitsRequest body,
        CancellationToken cancellationToken = default)
        => client.PostAsync<CreateConduitsRequest, CreateConduitsResponse>("eventsub/conduits", accessToken, body, queryParameters: null, cancellationToken);

    public Task<UpdateConduitsResponse> UpdateConduitsAsync(
        string accessToken,
        UpdateConduitsRequest body,
        CancellationToken cancellationToken = default)
        => client.PatchAsync<UpdateConduitsRequest, UpdateConduitsResponse>("eventsub/conduits", accessToken, body, queryParameters: null, cancellationToken);

    public Task DeleteConduitAsync(
        string accessToken,
        string id,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("id", id),
        };
        return client.DeleteAsync("eventsub/conduits", accessToken, query, cancellationToken);
    }

    public Task<GetConduitShardsResponse> GetConduitShardsAsync(
        string accessToken,
        string conduitId,
        string? status = null,
        string? after = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("conduit_id", conduitId),
        };
        query.AddIfNotNull("status", status);
        query.AddIfNotNull("after", after);
        return client.GetAsync<GetConduitShardsResponse>("eventsub/conduits/shards", accessToken, query, cancellationToken);
    }

    public Task<UpdateConduitShardsResponse> UpdateConduitShardsAsync(
        string accessToken,
        UpdateConduitShardsRequest body,
        CancellationToken cancellationToken = default)
        => client.PatchAsync<UpdateConduitShardsRequest, UpdateConduitShardsResponse>(
            "eventsub/conduits/shards", accessToken, body, queryParameters: null, cancellationToken);
}
