using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Charity.GetCharityCampaign;

public record GetCharityCampaignResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<CharityCampaign> Data
);
