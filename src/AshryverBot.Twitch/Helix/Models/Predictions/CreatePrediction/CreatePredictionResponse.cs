using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Predictions.Common;

namespace AshryverBot.Twitch.Helix.Models.Predictions.CreatePrediction;

public record CreatePredictionResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<Prediction> Data
);
