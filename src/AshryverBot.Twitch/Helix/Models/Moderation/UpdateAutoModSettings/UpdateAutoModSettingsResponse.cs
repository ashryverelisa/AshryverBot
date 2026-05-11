using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Moderation.GetAutoModSettings;

namespace AshryverBot.Twitch.Helix.Models.Moderation.UpdateAutoModSettings;

public record UpdateAutoModSettingsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<AutoModSettings> Data
);
