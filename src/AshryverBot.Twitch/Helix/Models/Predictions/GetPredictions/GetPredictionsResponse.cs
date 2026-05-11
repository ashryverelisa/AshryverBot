using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;
using AshryverBot.Twitch.Helix.Models.Predictions.Common;

namespace AshryverBot.Twitch.Helix.Models.Predictions.GetPredictions;

public record GetPredictionsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<Prediction> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
