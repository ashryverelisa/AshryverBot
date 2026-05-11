using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.GuestStar.Common;

namespace AshryverBot.Twitch.Helix.Models.GuestStar.EndGuestStarSession;

public record EndGuestStarSessionResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<GuestStarSession> Data
);
