using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Channels.GetChannelEditors;

public record ChannelEditor(
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("user_name")] string UserName,
    [property: JsonPropertyName("created_at")] DateTimeOffset CreatedAt
);
