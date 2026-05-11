using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Channels.Snooze;

public record Snooze
{
    [JsonPropertyName("snooze_count")]
    public int SnoozeCount { get; set; }

    [JsonPropertyName("snooze_refresh_at")]
    public string SnoozeRefreshAt { get; set; } = string.Empty;

    [JsonPropertyName("next_ad_at")]
    public string NextAdAt { get; set; } = string.Empty;
}