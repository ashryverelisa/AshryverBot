using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Predictions.EndPrediction;

public record EndPredictionRequest(
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("winning_outcome_id")] string? WinningOutcomeId
);
