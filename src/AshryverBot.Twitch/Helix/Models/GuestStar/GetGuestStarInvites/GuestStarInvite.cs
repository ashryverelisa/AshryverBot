using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.GuestStar.GetGuestStarInvites;

public record GuestStarInvite(
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("invited_at")] DateTimeOffset InvitedAt,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("is_video_enabled")] bool IsVideoEnabled,
    [property: JsonPropertyName("is_audio_enabled")] bool IsAudioEnabled,
    [property: JsonPropertyName("is_video_available")] bool IsVideoAvailable,
    [property: JsonPropertyName("is_audio_available")] bool IsAudioAvailable
);
