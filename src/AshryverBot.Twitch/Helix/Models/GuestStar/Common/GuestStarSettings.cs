using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.GuestStar.Common;

public record GuestStarSettings(
    [property: JsonPropertyName("is_moderator_send_live_enabled")] bool IsModeratorSendLiveEnabled,
    [property: JsonPropertyName("slot_count")] int SlotCount,
    [property: JsonPropertyName("is_browser_source_audio_enabled")] bool IsBrowserSourceAudioEnabled,
    [property: JsonPropertyName("group_layout")] string GroupLayout,
    [property: JsonPropertyName("browser_source_token")] string? BrowserSourceToken
);
