using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Bits.GetBitsLeaderboard;

public record BitsLeaderboardEntry(
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("user_login")] string UserLogin,
    [property: JsonPropertyName("user_name")] string UserName,
    [property: JsonPropertyName("rank")] int Rank,
    [property: JsonPropertyName("score")] long Score
);
