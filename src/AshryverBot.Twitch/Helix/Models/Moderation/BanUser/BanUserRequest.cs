using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Moderation.BanUser;

public record BanUserRequest(
    [property: JsonPropertyName("data")] BanUserPayload Data
);

public record BanUserPayload(
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("duration")] int? Duration,
    [property: JsonPropertyName("reason")] string? Reason
);
