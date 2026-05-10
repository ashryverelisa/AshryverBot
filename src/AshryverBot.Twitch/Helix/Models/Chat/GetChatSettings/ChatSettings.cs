using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Chat.GetChatSettings;

public record ChatSettings(
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("emote_mode")] bool EmoteMode,
    [property: JsonPropertyName("follower_mode")] bool FollowerMode,
    [property: JsonPropertyName("follower_mode_duration")] int? FollowerModeDuration,
    [property: JsonPropertyName("moderator_id")] string? ModeratorId,
    [property: JsonPropertyName("non_moderator_chat_delay")] bool? NonModeratorChatDelay,
    [property: JsonPropertyName("non_moderator_chat_delay_duration")] int? NonModeratorChatDelayDuration,
    [property: JsonPropertyName("slow_mode")] bool SlowMode,
    [property: JsonPropertyName("slow_mode_wait_time")] int? SlowModeWaitTime,
    [property: JsonPropertyName("subscriber_mode")] bool SubscriberMode,
    [property: JsonPropertyName("unique_chat_mode")] bool UniqueChatMode
);
