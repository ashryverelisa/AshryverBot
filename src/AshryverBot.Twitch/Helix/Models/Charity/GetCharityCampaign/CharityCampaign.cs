using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Charity.GetCharityCampaign;

public record CharityCampaign(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("broadcaster_login")] string BroadcasterLogin,
    [property: JsonPropertyName("broadcaster_name")] string BroadcasterName,
    [property: JsonPropertyName("charity_name")] string CharityName,
    [property: JsonPropertyName("charity_description")] string CharityDescription,
    [property: JsonPropertyName("charity_logo")] string CharityLogo,
    [property: JsonPropertyName("charity_website")] string CharityWebsite,
    [property: JsonPropertyName("current_amount")] CharityAmount CurrentAmount,
    [property: JsonPropertyName("target_amount")] CharityAmount TargetAmount
);

public record CharityAmount(
    [property: JsonPropertyName("value")] long Value,
    [property: JsonPropertyName("decimal_places")] int DecimalPlaces,
    [property: JsonPropertyName("currency")] string Currency
);
