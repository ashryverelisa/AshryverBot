using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Moderation.GetModerators;

public record GetModeratorsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<Moderator> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
