using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Schedule.Common;

public record StreamSchedule(
    [property: JsonPropertyName("segments")] IReadOnlyCollection<StreamScheduleSegment>? Segments,
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("broadcaster_name")] string BroadcasterName,
    [property: JsonPropertyName("broadcaster_login")] string BroadcasterLogin,
    [property: JsonPropertyName("vacation")] StreamScheduleVacation? Vacation
);

public record StreamScheduleSegment(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("start_time")] DateTimeOffset StartTime,
    [property: JsonPropertyName("end_time")] DateTimeOffset EndTime,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("canceled_until")] DateTimeOffset? CanceledUntil,
    [property: JsonPropertyName("category")] StreamScheduleCategory? Category,
    [property: JsonPropertyName("is_recurring")] bool IsRecurring
);

public record StreamScheduleCategory(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name
);

public record StreamScheduleVacation(
    [property: JsonPropertyName("start_time")] DateTimeOffset StartTime,
    [property: JsonPropertyName("end_time")] DateTimeOffset EndTime
);
