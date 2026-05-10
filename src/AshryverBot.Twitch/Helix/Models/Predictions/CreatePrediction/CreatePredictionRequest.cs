using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Predictions.CreatePrediction;

public record CreatePredictionRequest
{
    [JsonPropertyName("broadcaster_id")]
    public string BroadcasterId { get; init; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; init; } = string.Empty;

    [JsonPropertyName("outcomes")]
    public IReadOnlyCollection<CreatePredictionOutcome> Outcomes { get; init; } = [];

    [JsonPropertyName("prediction_window")]
    public int PredictionWindow { get; init; }
}

public record CreatePredictionOutcome(
    [property: JsonPropertyName("title")] string Title
);
