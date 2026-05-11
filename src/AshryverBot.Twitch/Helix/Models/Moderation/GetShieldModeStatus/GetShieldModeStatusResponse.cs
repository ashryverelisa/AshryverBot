using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Moderation.GetShieldModeStatus;

public record GetShieldModeStatusResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ShieldModeStatus> Data
);
