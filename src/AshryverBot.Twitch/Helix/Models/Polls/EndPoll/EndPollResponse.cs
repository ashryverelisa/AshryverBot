using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Polls.Common;

namespace AshryverBot.Twitch.Helix.Models.Polls.EndPoll;

public record EndPollResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<Poll> Data
);
