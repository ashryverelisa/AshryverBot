using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Bits.GetCheermotes;

public record GetCheermotesResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<Cheermote> Data
);
