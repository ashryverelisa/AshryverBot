using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Subscriptions.GetBroadcasterSubscriptions;

public record BroadcasterSubscription(
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("broadcaster_login")] string BroadcasterLogin,
    [property: JsonPropertyName("broadcaster_name")] string BroadcasterName,
    [property: JsonPropertyName("gifter_id")] string? GifterId,
    [property: JsonPropertyName("gifter_login")] string? GifterLogin,
    [property: JsonPropertyName("gifter_name")] string? GifterName,
    [property: JsonPropertyName("is_gift")] bool IsGift,
    [property: JsonPropertyName("plan_name")] string PlanName,
    [property: JsonPropertyName("tier")] string Tier,
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("user_login")] string UserLogin,
    [property: JsonPropertyName("user_name")] string UserName
);
