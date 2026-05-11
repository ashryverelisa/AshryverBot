using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Predictions.Common;

public record Prediction(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("broadcaster_name")] string BroadcasterName,
    [property: JsonPropertyName("broadcaster_login")] string BroadcasterLogin,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("winning_outcome_id")] string? WinningOutcomeId,
    [property: JsonPropertyName("outcomes")] IReadOnlyCollection<PredictionOutcome> Outcomes,
    [property: JsonPropertyName("prediction_window")] int PredictionWindow,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("created_at")] DateTimeOffset CreatedAt,
    [property: JsonPropertyName("ended_at")] DateTimeOffset? EndedAt,
    [property: JsonPropertyName("locked_at")] DateTimeOffset? LockedAt
);

public record PredictionOutcome(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("users")] int Users,
    [property: JsonPropertyName("channel_points")] long ChannelPoints,
    [property: JsonPropertyName("top_predictors")] IReadOnlyCollection<TopPredictor>? TopPredictors,
    [property: JsonPropertyName("color")] string Color
);

public record TopPredictor(
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("user_login")] string UserLogin,
    [property: JsonPropertyName("user_name")] string UserName,
    [property: JsonPropertyName("channel_points_used")] long ChannelPointsUsed,
    [property: JsonPropertyName("channel_points_won")] long? ChannelPointsWon
);
