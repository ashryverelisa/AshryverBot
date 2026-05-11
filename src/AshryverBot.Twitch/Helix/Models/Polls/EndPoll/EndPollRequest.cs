using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Polls.EndPoll;

public record EndPollRequest(
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("status")] string Status
);
