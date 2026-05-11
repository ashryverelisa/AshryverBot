using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Chat.GetChatters;

public record GetChattersResponse(
    [property: JsonPropertyName("data")] Chatter[] Data,
    [property: JsonPropertyName("pagination")] Pagination Pagination,
    [property: JsonPropertyName("total")] int Total
);