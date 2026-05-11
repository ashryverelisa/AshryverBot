using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Moderation.GetModerators;

public record Moderator(
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("user_login")] string UserLogin,
    [property: JsonPropertyName("user_name")] string UserName
);
