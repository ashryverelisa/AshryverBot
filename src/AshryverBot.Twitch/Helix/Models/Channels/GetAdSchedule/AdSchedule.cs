using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Channels.GetAdSchedule;

public record AdSchedule
{
    [JsonPropertyName("snooze_count")]
    public int SnoozeCount { get; set; }

    [JsonPropertyName("snooze_refresh_at")]
    public string SnoozeRefreshAt { get;  set; } = string.Empty;

    [JsonPropertyName("next_ad_at")]
    public string NextAdAt { get; set; } = string.Empty;

    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    [JsonPropertyName("last_ad_at")]
    public string LastAdAt { get; set; } = string.Empty;

    [JsonPropertyName("preroll_free_time")]
    public int PrerollFreeTime { get; set; }
}