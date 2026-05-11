using AshryverBot.Twitch.Helix.Models.Charity.GetCharityCampaign;
using AshryverBot.Twitch.Helix.Models.Charity.GetCharityCampaignDonations;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface ICharityApi
{
    Task<GetCharityCampaignResponse> GetCharityCampaignAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default);

    Task<GetCharityCampaignDonationsResponse> GetCharityCampaignDonationsAsync(
        string accessToken,
        string broadcasterId,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default);
}
