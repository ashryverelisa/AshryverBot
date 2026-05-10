using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Conduits.GetConduits;

namespace AshryverBot.Twitch.Helix.Models.Conduits.CreateConduits;

public record CreateConduitsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<Conduit> Data
);
