using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.GuestStar.Common;

namespace AshryverBot.Twitch.Helix.Models.GuestStar.GetGuestStarSession;

public record GetGuestStarSessionResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<GuestStarSession> Data
);
