using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.ContentClassificationLabels.GetContentClassificationLabels;

public record ContentClassificationLabel(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("name")] string Name
);
