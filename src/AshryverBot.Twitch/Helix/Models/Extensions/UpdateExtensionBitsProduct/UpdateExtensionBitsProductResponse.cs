using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Extensions.GetExtensionBitsProducts;

namespace AshryverBot.Twitch.Helix.Models.Extensions.UpdateExtensionBitsProduct;

public record UpdateExtensionBitsProductResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ExtensionBitsProduct> Data
);
