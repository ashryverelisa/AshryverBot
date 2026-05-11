using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Analytics.GetExtensionAnalytics;

public record GetExtensionAnalyticsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ExtensionAnalytic> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
