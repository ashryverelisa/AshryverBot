using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.ChannelPoints.GetCustomRewardRedemption;

namespace AshryverBot.Twitch.Helix.Models.ChannelPoints.UpdateRedemptionStatus;

public record UpdateRedemptionStatusResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<CustomRewardRedemption> Data
);
