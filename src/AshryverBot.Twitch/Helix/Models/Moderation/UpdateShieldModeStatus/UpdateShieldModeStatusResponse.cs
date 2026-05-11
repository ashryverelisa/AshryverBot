using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Moderation.GetShieldModeStatus;

namespace AshryverBot.Twitch.Helix.Models.Moderation.UpdateShieldModeStatus;

public record UpdateShieldModeStatusResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ShieldModeStatus> Data
);
