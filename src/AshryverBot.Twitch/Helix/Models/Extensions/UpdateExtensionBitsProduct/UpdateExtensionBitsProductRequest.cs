using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Extensions.GetExtensionBitsProducts;

namespace AshryverBot.Twitch.Helix.Models.Extensions.UpdateExtensionBitsProduct;

public record UpdateExtensionBitsProductRequest(
    [property: JsonPropertyName("sku")] string Sku,
    [property: JsonPropertyName("cost")] ExtensionBitsProductCost Cost,
    [property: JsonPropertyName("display_name")] string DisplayName,
    [property: JsonPropertyName("in_development")] bool? InDevelopment,
    [property: JsonPropertyName("expiration")] DateTimeOffset? Expiration,
    [property: JsonPropertyName("is_broadcast")] bool? IsBroadcast
);
