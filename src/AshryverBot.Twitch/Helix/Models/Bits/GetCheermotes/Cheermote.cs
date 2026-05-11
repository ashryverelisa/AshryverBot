using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Bits.GetCheermotes;

public record Cheermote(
    [property: JsonPropertyName("prefix")] string Prefix,
    [property: JsonPropertyName("tiers")] IReadOnlyCollection<CheermoteTier> Tiers,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("order")] int Order,
    [property: JsonPropertyName("last_updated")] DateTimeOffset LastUpdated,
    [property: JsonPropertyName("is_charitable")] bool IsCharitable
);

public record CheermoteTier(
    [property: JsonPropertyName("min_bits")] int MinBits,
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("color")] string Color,
    [property: JsonPropertyName("images")] CheermoteImages Images,
    [property: JsonPropertyName("can_cheer")] bool CanCheer,
    [property: JsonPropertyName("show_in_bits_card")] bool ShowInBitsCard
);

public record CheermoteImages(
    [property: JsonPropertyName("dark")] CheermoteImageVariant Dark,
    [property: JsonPropertyName("light")] CheermoteImageVariant Light
);

public record CheermoteImageVariant(
    [property: JsonPropertyName("animated")] IReadOnlyDictionary<string, string> Animated,
    [property: JsonPropertyName("static")] IReadOnlyDictionary<string, string> Static
);
