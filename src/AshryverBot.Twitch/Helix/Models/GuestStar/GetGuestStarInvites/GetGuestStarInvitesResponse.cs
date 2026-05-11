using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.GuestStar.GetGuestStarInvites;

public record GetGuestStarInvitesResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<GuestStarInvite> Data
);
