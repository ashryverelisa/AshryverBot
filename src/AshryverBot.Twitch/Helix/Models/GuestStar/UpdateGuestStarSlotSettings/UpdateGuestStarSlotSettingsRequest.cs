using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.GuestStar.UpdateGuestStarSlotSettings;

public record UpdateGuestStarSlotSettingsRequest
{
    [JsonPropertyName("is_audio_enabled")]
    public bool? IsAudioEnabled { get; init; }

    [JsonPropertyName("is_video_enabled")]
    public bool? IsVideoEnabled { get; init; }

    [JsonPropertyName("is_live")]
    public bool? IsLive { get; init; }

    [JsonPropertyName("volume")]
    public int? Volume { get; init; }
}
