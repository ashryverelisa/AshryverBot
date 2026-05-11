using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Chat.GetGlobalEmotes;

public record GetGlobalEmotesResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<GlobalEmote> Data,
    [property: JsonPropertyName("template")] string Template
);
