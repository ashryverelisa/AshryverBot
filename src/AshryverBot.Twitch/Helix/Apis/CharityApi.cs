using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.Charity.GetCharityCampaign;
using AshryverBot.Twitch.Helix.Models.Charity.GetCharityCampaignDonations;

namespace AshryverBot.Twitch.Helix.Apis;

public class CharityApi(ITwitchClient client) : ICharityApi
{
    public Task<GetCharityCampaignResponse> GetCharityCampaignAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        return client.GetAsync<GetCharityCampaignResponse>("charity/campaigns", accessToken, query, cancellationToken);
    }

    public Task<GetCharityCampaignDonationsResponse> GetCharityCampaignDonationsAsync(
        string accessToken,
        string broadcasterId,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        query.AddIfNotNull("first", first);
        query.AddIfNotNull("after", after);
        return client.GetAsync<GetCharityCampaignDonationsResponse>("charity/donations", accessToken, query, cancellationToken);
    }
}
