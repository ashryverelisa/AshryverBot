using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Moderation.GetAutoModSettings;

public record GetAutoModSettingsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<AutoModSettings> Data
);
