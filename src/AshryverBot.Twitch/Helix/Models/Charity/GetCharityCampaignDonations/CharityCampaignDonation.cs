using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Charity.GetCharityCampaign;

namespace AshryverBot.Twitch.Helix.Models.Charity.GetCharityCampaignDonations;

public record CharityCampaignDonation(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("campaign_id")] string CampaignId,
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("user_login")] string UserLogin,
    [property: JsonPropertyName("user_name")] string UserName,
    [property: JsonPropertyName("amount")] CharityAmount Amount
);
