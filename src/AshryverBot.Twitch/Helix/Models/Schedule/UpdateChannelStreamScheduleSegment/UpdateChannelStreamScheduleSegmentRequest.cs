using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Schedule.UpdateChannelStreamScheduleSegment;

public record UpdateChannelStreamScheduleSegmentRequest
{
    [JsonPropertyName("start_time")]
    public DateTimeOffset? StartTime { get; init; }

    [JsonPropertyName("duration")]
    public string? Duration { get; init; }

    [JsonPropertyName("category_id")]
    public string? CategoryId { get; init; }

    [JsonPropertyName("title")]
    public string? Title { get; init; }

    [JsonPropertyName("is_canceled")]
    public bool? IsCanceled { get; init; }

    [JsonPropertyName("timezone")]
    public string? Timezone { get; init; }
}
