using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Channels.GetChannelFollowers;

public record ChannelFollower(
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("user_login")] string UserLogin,
    [property: JsonPropertyName("user_name")] string UserName,
    [property: JsonPropertyName("followed_at")] DateTimeOffset FollowedAt
);
