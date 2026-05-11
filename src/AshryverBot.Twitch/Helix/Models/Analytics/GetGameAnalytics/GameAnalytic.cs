using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Analytics.GetGameAnalytics;

public record GameAnalytic(
    [property: JsonPropertyName("game_id")] string GameId,
    [property: JsonPropertyName("URL")] string Url,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("date_range")] DateRange DateRange
);
