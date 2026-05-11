using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.ChannelPoints.UpdateCustomReward;

public record UpdateCustomRewardRequest
{
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    [JsonPropertyName("prompt")]
    public string? Prompt { get; init; }

    [JsonPropertyName("cost")]
    public long? Cost { get; init; }

    [JsonPropertyName("background_color")]
    public string? BackgroundColor { get; init; }

    [JsonPropertyName("is_enabled")]
    public bool? IsEnabled { get; init; }

    [JsonPropertyName("is_user_input_required")]
    public bool? IsUserInputRequired { get; init; }

    [JsonPropertyName("is_max_per_stream_enabled")]
    public bool? IsMaxPerStreamEnabled { get; init; }

    [JsonPropertyName("max_per_stream")]
    public int? MaxPerStream { get; init; }

    [JsonPropertyName("is_max_per_user_per_stream_enabled")]
    public bool? IsMaxPerUserPerStreamEnabled { get; init; }

    [JsonPropertyName("max_per_user_per_stream")]
    public int? MaxPerUserPerStream { get; init; }

    [JsonPropertyName("is_global_cooldown_enabled")]
    public bool? IsGlobalCooldownEnabled { get; init; }

    [JsonPropertyName("global_cooldown_seconds")]
    public int? GlobalCooldownSeconds { get; init; }

    [JsonPropertyName("is_paused")]
    public bool? IsPaused { get; init; }

    [JsonPropertyName("should_redemptions_skip_request_queue")]
    public bool? ShouldRedemptionsSkipRequestQueue { get; init; }
}
