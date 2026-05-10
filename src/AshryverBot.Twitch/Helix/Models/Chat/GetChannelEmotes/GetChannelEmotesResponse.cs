using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Chat.GetChannelEmotes;

public record GetChannelEmotesResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ChannelEmote> Data,
    [property: JsonPropertyName("template")] string Template
);
