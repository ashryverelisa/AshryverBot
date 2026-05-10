using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Subscriptions.CheckUserSubscription;

public record UserSubscription(
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("broadcaster_login")] string BroadcasterLogin,
    [property: JsonPropertyName("broadcaster_name")] string BroadcasterName,
    [property: JsonPropertyName("gifter_id")] string? GifterId,
    [property: JsonPropertyName("gifter_login")] string? GifterLogin,
    [property: JsonPropertyName("gifter_name")] string? GifterName,
    [property: JsonPropertyName("is_gift")] bool IsGift,
    [property: JsonPropertyName("tier")] string Tier
);
