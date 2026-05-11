using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Extensions.GetExtensionBitsProducts;

public record GetExtensionBitsProductsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ExtensionBitsProduct> Data
);
