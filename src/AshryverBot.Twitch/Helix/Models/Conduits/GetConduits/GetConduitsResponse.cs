using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Conduits.GetConduits;

public record GetConduitsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<Conduit> Data
);
