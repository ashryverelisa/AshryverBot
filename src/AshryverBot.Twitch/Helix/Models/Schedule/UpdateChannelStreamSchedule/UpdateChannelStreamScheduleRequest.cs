using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Schedule.UpdateChannelStreamSchedule;

public record UpdateChannelStreamScheduleRequest
{
    [JsonPropertyName("is_vacation_enabled")]
    public bool? IsVacationEnabled { get; init; }

    [JsonPropertyName("vacation_start_time")]
    public DateTimeOffset? VacationStartTime { get; init; }

    [JsonPropertyName("vacation_end_time")]
    public DateTimeOffset? VacationEndTime { get; init; }

    [JsonPropertyName("timezone")]
    public string? Timezone { get; init; }
}
