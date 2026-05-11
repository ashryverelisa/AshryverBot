using AshryverBot.Twitch.Helix.Models.Entitlements.GetDropsEntitlements;
using AshryverBot.Twitch.Helix.Models.Entitlements.UpdateDropsEntitlements;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IEntitlementsApi
{
    Task<GetDropsEntitlementsResponse> GetDropsEntitlementsAsync(
        string accessToken,
        IEnumerable<string>? ids = null,
        string? userId = null,
        string? gameId = null,
        string? fulfillmentStatus = null,
        string? after = null,
        int? first = null,
        CancellationToken cancellationToken = default);

    Task<UpdateDropsEntitlementsResponse> UpdateDropsEntitlementsAsync(
        string accessToken,
        UpdateDropsEntitlementsRequest body,
        CancellationToken cancellationToken = default);
}
