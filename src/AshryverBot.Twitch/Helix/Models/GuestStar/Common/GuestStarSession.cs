using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.GuestStar.Common;

public record GuestStarSession(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("guests")] IReadOnlyCollection<GuestStarGuest> Guests
);

public record GuestStarGuest(
    [property: JsonPropertyName("slot_id")] string SlotId,
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("user_display_name")] string UserDisplayName,
    [property: JsonPropertyName("user_login")] string UserLogin,
    [property: JsonPropertyName("is_live")] bool IsLive,
    [property: JsonPropertyName("volume")] int Volume,
    [property: JsonPropertyName("assigned_at")] DateTimeOffset AssignedAt,
    [property: JsonPropertyName("audio_settings")] GuestStarMediaSettings AudioSettings,
    [property: JsonPropertyName("video_settings")] GuestStarMediaSettings VideoSettings
);

public record GuestStarMediaSettings(
    [property: JsonPropertyName("is_available")] bool IsAvailable,
    [property: JsonPropertyName("is_host_enabled")] bool IsHostEnabled,
    [property: JsonPropertyName("is_guest_enabled")] bool IsGuestEnabled
);
