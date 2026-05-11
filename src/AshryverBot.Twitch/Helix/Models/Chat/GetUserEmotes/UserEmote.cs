using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Chat.GetUserEmotes;

public record UserEmote(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("emote_type")] string EmoteType,
    [property: JsonPropertyName("emote_set_id")] string EmoteSetId,
    [property: JsonPropertyName("owner_id")] string OwnerId,
    [property: JsonPropertyName("format")] IReadOnlyCollection<string> Format,
    [property: JsonPropertyName("scale")] IReadOnlyCollection<string> Scale,
    [property: JsonPropertyName("theme_mode")] IReadOnlyCollection<string> ThemeMode
);
