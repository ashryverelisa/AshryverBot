using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.GuestStar.Common;

namespace AshryverBot.Twitch.Helix.Models.GuestStar.CreateGuestStarSession;

public record CreateGuestStarSessionResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<GuestStarSession> Data
);
