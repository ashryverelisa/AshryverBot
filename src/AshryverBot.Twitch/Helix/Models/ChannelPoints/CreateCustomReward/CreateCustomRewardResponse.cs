using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.ChannelPoints.Common;

namespace AshryverBot.Twitch.Helix.Models.ChannelPoints.CreateCustomReward;

public record CreateCustomRewardResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<CustomReward> Data
);
