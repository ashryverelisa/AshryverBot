using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Charity.GetCharityCampaignDonations;

public record GetCharityCampaignDonationsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<CharityCampaignDonation> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
