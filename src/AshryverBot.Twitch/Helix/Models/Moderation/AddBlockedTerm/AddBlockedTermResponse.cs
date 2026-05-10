using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Moderation.GetBlockedTerms;

namespace AshryverBot.Twitch.Helix.Models.Moderation.AddBlockedTerm;

public record AddBlockedTermResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<BlockedTerm> Data
);
