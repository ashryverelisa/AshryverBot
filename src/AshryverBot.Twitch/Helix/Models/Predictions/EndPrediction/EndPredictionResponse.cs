using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Predictions.Common;

namespace AshryverBot.Twitch.Helix.Models.Predictions.EndPrediction;

public record EndPredictionResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<Prediction> Data
);
