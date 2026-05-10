using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Chat.GetUserChatColor;

public record UserChatColor(
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("user_login")] string UserLogin,
    [property: JsonPropertyName("user_name")] string UserName,
    [property: JsonPropertyName("color")] string Color
);
