using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Goals.GetCreatorGoals;

public record CreatorGoal(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("broadcaster_name")] string BroadcasterName,
    [property: JsonPropertyName("broadcaster_login")] string BroadcasterLogin,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("current_amount")] long CurrentAmount,
    [property: JsonPropertyName("target_amount")] long TargetAmount,
    [property: JsonPropertyName("created_at")] DateTimeOffset CreatedAt
);
