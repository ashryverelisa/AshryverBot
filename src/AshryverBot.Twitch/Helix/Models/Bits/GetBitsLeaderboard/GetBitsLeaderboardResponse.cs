using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Bits.GetBitsLeaderboard;

public record GetBitsLeaderboardResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<BitsLeaderboardEntry> Data,
    [property: JsonPropertyName("date_range")] DateRange? DateRange,
    [property: JsonPropertyName("total")] int Total
);
