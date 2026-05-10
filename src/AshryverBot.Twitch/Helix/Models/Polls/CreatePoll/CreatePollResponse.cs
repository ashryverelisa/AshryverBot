using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Polls.Common;

namespace AshryverBot.Twitch.Helix.Models.Polls.CreatePoll;

public record CreatePollResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<Poll> Data
);
