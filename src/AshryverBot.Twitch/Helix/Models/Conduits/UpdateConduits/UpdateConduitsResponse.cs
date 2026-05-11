using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Conduits.GetConduits;

namespace AshryverBot.Twitch.Helix.Models.Conduits.UpdateConduits;

public record UpdateConduitsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<Conduit> Data
);
