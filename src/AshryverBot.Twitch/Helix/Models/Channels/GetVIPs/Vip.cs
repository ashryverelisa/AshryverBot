using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Channels.GetVIPs;

public record Vip(
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("user_name")] string UserName,
    [property: JsonPropertyName("user_login")] string UserLogin
);
