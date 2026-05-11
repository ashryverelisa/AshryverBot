using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Chat.GetUserEmotes;

public record GetUserEmotesResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<UserEmote> Data,
    [property: JsonPropertyName("template")] string Template,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
