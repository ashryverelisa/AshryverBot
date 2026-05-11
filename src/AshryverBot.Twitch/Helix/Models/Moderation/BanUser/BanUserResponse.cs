using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Moderation.BanUser;

public record BanUserResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<BanUserResult> Data
);
