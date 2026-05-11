using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Moderation.GetBlockedTerms;

public record GetBlockedTermsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<BlockedTerm> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
