using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Chat.GetEmoteSets;

public record GetEmoteSetsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<EmoteSetEmote> Data,
    [property: JsonPropertyName("template")] string Template
);
