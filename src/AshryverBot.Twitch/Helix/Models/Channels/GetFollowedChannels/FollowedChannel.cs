using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Channels.GetFollowedChannels;

public record FollowedChannel(
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("broadcaster_login")] string BroadcasterLogin,
    [property: JsonPropertyName("broadcaster_name")] string BroadcasterName,
    [property: JsonPropertyName("followed_at")] DateTimeOffset FollowedAt
);
