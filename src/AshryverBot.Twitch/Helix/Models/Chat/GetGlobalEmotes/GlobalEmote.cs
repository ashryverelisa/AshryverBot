using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Chat.GetChannelEmotes;

namespace AshryverBot.Twitch.Helix.Models.Chat.GetGlobalEmotes;

public record GlobalEmote(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("images")] EmoteImages Images,
    [property: JsonPropertyName("format")] IReadOnlyCollection<string> Format,
    [property: JsonPropertyName("scale")] IReadOnlyCollection<string> Scale,
    [property: JsonPropertyName("theme_mode")] IReadOnlyCollection<string> ThemeMode
);
