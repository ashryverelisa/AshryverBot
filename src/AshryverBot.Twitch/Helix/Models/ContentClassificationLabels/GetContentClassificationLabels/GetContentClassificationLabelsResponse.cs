using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.ContentClassificationLabels.GetContentClassificationLabels;

public record GetContentClassificationLabelsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ContentClassificationLabel> Data
);
