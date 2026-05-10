using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.GuestStar.Common;

namespace AshryverBot.Twitch.Helix.Models.GuestStar.GetChannelGuestStarSettings;

public record GetChannelGuestStarSettingsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<GuestStarSettings> Data
);
