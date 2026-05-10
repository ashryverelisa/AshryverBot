using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Moderation.AddBlockedTerm;

public record AddBlockedTermRequest(
    [property: JsonPropertyName("text")] string Text
);
