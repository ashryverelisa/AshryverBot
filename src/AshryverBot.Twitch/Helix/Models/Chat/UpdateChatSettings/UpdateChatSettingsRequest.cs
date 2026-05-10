using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Chat.UpdateChatSettings;

public record UpdateChatSettingsRequest
{
    [JsonPropertyName("emote_mode")]
    public bool? EmoteMode { get; init; }

    [JsonPropertyName("follower_mode")]
    public bool? FollowerMode { get; init; }

    [JsonPropertyName("follower_mode_duration")]
    public int? FollowerModeDuration { get; init; }

    [JsonPropertyName("non_moderator_chat_delay")]
    public bool? NonModeratorChatDelay { get; init; }

    [JsonPropertyName("non_moderator_chat_delay_duration")]
    public int? NonModeratorChatDelayDuration { get; init; }

    [JsonPropertyName("slow_mode")]
    public bool? SlowMode { get; init; }

    [JsonPropertyName("slow_mode_wait_time")]
    public int? SlowModeWaitTime { get; init; }

    [JsonPropertyName("subscriber_mode")]
    public bool? SubscriberMode { get; init; }

    [JsonPropertyName("unique_chat_mode")]
    public bool? UniqueChatMode { get; init; }
}
