using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Chat.GetChannelEmotes;

public record ChannelEmote(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("images")] EmoteImages Images,
    [property: JsonPropertyName("tier")] string Tier,
    [property: JsonPropertyName("emote_type")] string EmoteType,
    [property: JsonPropertyName("emote_set_id")] string EmoteSetId,
    [property: JsonPropertyName("format")] IReadOnlyCollection<string> Format,
    [property: JsonPropertyName("scale")] IReadOnlyCollection<string> Scale,
    [property: JsonPropertyName("theme_mode")] IReadOnlyCollection<string> ThemeMode
);

public record EmoteImages(
    [property: JsonPropertyName("url_1x")] string Url1x,
    [property: JsonPropertyName("url_2x")] string Url2x,
    [property: JsonPropertyName("url_4x")] string Url4x
);
