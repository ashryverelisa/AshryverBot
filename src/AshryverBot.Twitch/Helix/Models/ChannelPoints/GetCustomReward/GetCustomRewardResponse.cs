using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.ChannelPoints.Common;

namespace AshryverBot.Twitch.Helix.Models.ChannelPoints.GetCustomReward;

public record GetCustomRewardResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<CustomReward> Data
);
