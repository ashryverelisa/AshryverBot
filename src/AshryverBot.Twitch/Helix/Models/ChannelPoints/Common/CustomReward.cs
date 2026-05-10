using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.ChannelPoints.Common;

public record CustomReward(
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("broadcaster_login")] string BroadcasterLogin,
    [property: JsonPropertyName("broadcaster_name")] string BroadcasterName,
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("prompt")] string Prompt,
    [property: JsonPropertyName("cost")] long Cost,
    [property: JsonPropertyName("image")] CustomRewardImage? Image,
    [property: JsonPropertyName("default_image")] CustomRewardImage DefaultImage,
    [property: JsonPropertyName("background_color")] string BackgroundColor,
    [property: JsonPropertyName("is_enabled")] bool IsEnabled,
    [property: JsonPropertyName("is_user_input_required")] bool IsUserInputRequired,
    [property: JsonPropertyName("max_per_stream_setting")] CustomRewardMaxPerStream MaxPerStreamSetting,
    [property: JsonPropertyName("max_per_user_per_stream_setting")] CustomRewardMaxPerUserPerStream MaxPerUserPerStreamSetting,
    [property: JsonPropertyName("global_cooldown_setting")] CustomRewardGlobalCooldown GlobalCooldownSetting,
    [property: JsonPropertyName("is_paused")] bool IsPaused,
    [property: JsonPropertyName("is_in_stock")] bool IsInStock,
    [property: JsonPropertyName("should_redemptions_skip_request_queue")] bool ShouldRedemptionsSkipRequestQueue,
    [property: JsonPropertyName("redemptions_redeemed_current_stream")] int? RedemptionsRedeemedCurrentStream,
    [property: JsonPropertyName("cooldown_expires_at")] DateTimeOffset? CooldownExpiresAt
);

public record CustomRewardImage(
    [property: JsonPropertyName("url_1x")] string Url1x,
    [property: JsonPropertyName("url_2x")] string Url2x,
    [property: JsonPropertyName("url_4x")] string Url4x
);

public record CustomRewardMaxPerStream(
    [property: JsonPropertyName("is_enabled")] bool IsEnabled,
    [property: JsonPropertyName("max_per_stream")] int MaxPerStream
);

public record CustomRewardMaxPerUserPerStream(
    [property: JsonPropertyName("is_enabled")] bool IsEnabled,
    [property: JsonPropertyName("max_per_user_per_stream")] int MaxPerUserPerStream
);

public record CustomRewardGlobalCooldown(
    [property: JsonPropertyName("is_enabled")] bool IsEnabled,
    [property: JsonPropertyName("global_cooldown_seconds")] int GlobalCooldownSeconds
);
