using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Raids.StartARaid;

public record Raid(
    [property: JsonPropertyName("created_at")] DateTimeOffset CreatedAt,
    [property: JsonPropertyName("is_mature")] bool IsMature
);
