using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.ChannelPoints.GetCustomRewardRedemption;

public record CustomRewardRedemption(
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("broadcaster_login")] string BroadcasterLogin,
    [property: JsonPropertyName("broadcaster_name")] string BroadcasterName,
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("user_login")] string UserLogin,
    [property: JsonPropertyName("user_name")] string UserName,
    [property: JsonPropertyName("user_input")] string UserInput,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("redeemed_at")] DateTimeOffset RedeemedAt,
    [property: JsonPropertyName("reward")] RedemptionReward Reward
);

public record RedemptionReward(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("prompt")] string Prompt,
    [property: JsonPropertyName("cost")] long Cost
);
