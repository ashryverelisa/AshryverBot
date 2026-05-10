using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.GuestStar.UpdateChannelGuestStarSettings;

public record UpdateChannelGuestStarSettingsRequest
{
    [JsonPropertyName("is_moderator_send_live_enabled")]
    public bool? IsModeratorSendLiveEnabled { get; init; }

    [JsonPropertyName("slot_count")]
    public int? SlotCount { get; init; }

    [JsonPropertyName("is_browser_source_audio_enabled")]
    public bool? IsBrowserSourceAudioEnabled { get; init; }

    [JsonPropertyName("group_layout")]
    public string? GroupLayout { get; init; }

    [JsonPropertyName("regenerate_browser_sources")]
    public bool? RegenerateBrowserSources { get; init; }
}
