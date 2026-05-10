using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.ChannelPoints.GetCustomRewardRedemption;

public record GetCustomRewardRedemptionResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<CustomRewardRedemption> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
