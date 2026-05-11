using AshryverBot.Twitch.Helix.Models.Conduits.CreateConduits;
using AshryverBot.Twitch.Helix.Models.Conduits.GetConduits;
using AshryverBot.Twitch.Helix.Models.Conduits.GetConduitShards;
using AshryverBot.Twitch.Helix.Models.Conduits.UpdateConduits;
using AshryverBot.Twitch.Helix.Models.Conduits.UpdateConduitShards;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IConduitsApi
{
    Task<GetConduitsResponse> GetConduitsAsync(
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<CreateConduitsResponse> CreateConduitsAsync(
        string accessToken,
        CreateConduitsRequest body,
        CancellationToken cancellationToken = default);

    Task<UpdateConduitsResponse> UpdateConduitsAsync(
        string accessToken,
        UpdateConduitsRequest body,
        CancellationToken cancellationToken = default);

    Task DeleteConduitAsync(
        string accessToken,
        string id,
        CancellationToken cancellationToken = default);

    Task<GetConduitShardsResponse> GetConduitShardsAsync(
        string accessToken,
        string conduitId,
        string? status = null,
        string? after = null,
        CancellationToken cancellationToken = default);

    Task<UpdateConduitShardsResponse> UpdateConduitShardsAsync(
        string accessToken,
        UpdateConduitShardsRequest body,
        CancellationToken cancellationToken = default);
}
