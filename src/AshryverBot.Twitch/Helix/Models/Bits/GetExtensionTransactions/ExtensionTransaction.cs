using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Bits.GetExtensionTransactions;

public record ExtensionTransaction(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("timestamp")] DateTimeOffset Timestamp,
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("broadcaster_login")] string BroadcasterLogin,
    [property: JsonPropertyName("broadcaster_name")] string BroadcasterName,
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("user_login")] string UserLogin,
    [property: JsonPropertyName("user_name")] string UserName,
    [property: JsonPropertyName("product_type")] string ProductType,
    [property: JsonPropertyName("product_data")] ExtensionProductData ProductData
);

public record ExtensionProductData(
    [property: JsonPropertyName("sku")] string Sku,
    [property: JsonPropertyName("domain")] string Domain,
    [property: JsonPropertyName("cost")] ExtensionProductCost Cost,
    [property: JsonPropertyName("inDevelopment")] bool InDevelopment,
    [property: JsonPropertyName("displayName")] string DisplayName,
    [property: JsonPropertyName("expiration")] string Expiration,
    [property: JsonPropertyName("broadcast")] bool Broadcast
);

public record ExtensionProductCost(
    [property: JsonPropertyName("amount")] int Amount,
    [property: JsonPropertyName("type")] string Type
);
