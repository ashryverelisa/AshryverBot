using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Goals.GetCreatorGoals;

public record GetCreatorGoalsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<CreatorGoal> Data
);
