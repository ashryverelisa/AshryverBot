using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Raids.StartARaid;

public record StartARaidResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<Raid> Data
);
