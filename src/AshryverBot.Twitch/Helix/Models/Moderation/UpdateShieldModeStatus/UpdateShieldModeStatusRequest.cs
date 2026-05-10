using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Moderation.UpdateShieldModeStatus;

public record UpdateShieldModeStatusRequest(
    [property: JsonPropertyName("is_active")] bool IsActive
);
