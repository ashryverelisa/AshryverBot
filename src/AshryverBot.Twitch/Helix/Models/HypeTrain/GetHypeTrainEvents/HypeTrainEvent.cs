using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.HypeTrain.GetHypeTrainEvents;

public record HypeTrainEvent(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("event_type")] string EventType,
    [property: JsonPropertyName("event_timestamp")] DateTimeOffset EventTimestamp,
    [property: JsonPropertyName("version")] string Version,
    [property: JsonPropertyName("event_data")] HypeTrainEventData EventData
);

public record HypeTrainEventData(
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("cooldown_end_time")] DateTimeOffset CooldownEndTime,
    [property: JsonPropertyName("expires_at")] DateTimeOffset ExpiresAt,
    [property: JsonPropertyName("goal")] long Goal,
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("last_contribution")] HypeTrainContribution LastContribution,
    [property: JsonPropertyName("level")] int Level,
    [property: JsonPropertyName("started_at")] DateTimeOffset StartedAt,
    [property: JsonPropertyName("top_contributions")] IReadOnlyCollection<HypeTrainContribution> TopContributions,
    [property: JsonPropertyName("total")] long Total
);

public record HypeTrainContribution(
    [property: JsonPropertyName("total")] long Total,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("user")] string User
);
