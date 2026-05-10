using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Schedule.CreateChannelStreamScheduleSegment;

public record CreateChannelStreamScheduleSegmentRequest
{
    [JsonPropertyName("start_time")]
    public DateTimeOffset StartTime { get; init; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; init; } = string.Empty;

    [JsonPropertyName("duration")]
    public string Duration { get; init; } = string.Empty;

    [JsonPropertyName("is_recurring")]
    public bool? IsRecurring { get; init; }

    [JsonPropertyName("category_id")]
    public string? CategoryId { get; init; }

    [JsonPropertyName("title")]
    public string? Title { get; init; }
}
