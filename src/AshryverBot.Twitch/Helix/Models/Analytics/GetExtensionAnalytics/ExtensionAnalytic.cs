using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Analytics.GetExtensionAnalytics;

public record ExtensionAnalytic(
    [property: JsonPropertyName("extension_id")] string ExtensionId,
    [property: JsonPropertyName("URL")] string Url,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("date_range")] DateRange DateRange
);
