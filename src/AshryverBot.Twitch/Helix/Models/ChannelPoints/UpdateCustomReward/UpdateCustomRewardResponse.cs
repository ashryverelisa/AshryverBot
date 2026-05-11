using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.ChannelPoints.Common;

namespace AshryverBot.Twitch.Helix.Models.ChannelPoints.UpdateCustomReward;

public record UpdateCustomRewardResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<CustomReward> Data
);
