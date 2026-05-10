using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Analytics.GetGameAnalytics;

public record GetGameAnalyticsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<GameAnalytic> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
