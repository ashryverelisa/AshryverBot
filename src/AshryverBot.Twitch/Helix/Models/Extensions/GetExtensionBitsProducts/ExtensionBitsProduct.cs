using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Extensions.GetExtensionBitsProducts;

public record ExtensionBitsProduct(
    [property: JsonPropertyName("sku")] string Sku,
    [property: JsonPropertyName("cost")] ExtensionBitsProductCost Cost,
    [property: JsonPropertyName("in_development")] bool InDevelopment,
    [property: JsonPropertyName("display_name")] string DisplayName,
    [property: JsonPropertyName("expiration")] DateTimeOffset Expiration,
    [property: JsonPropertyName("is_broadcast")] bool IsBroadcast
);

public record ExtensionBitsProductCost(
    [property: JsonPropertyName("amount")] int Amount,
    [property: JsonPropertyName("type")] string Type
);
