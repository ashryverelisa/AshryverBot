using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Users.GetUserBlockList;

public record BlockedUser(
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("user_login")] string UserLogin,
    [property: JsonPropertyName("display_name")] string DisplayName
);
