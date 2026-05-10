using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;
using AshryverBot.Twitch.Helix.Models.Polls.Common;

namespace AshryverBot.Twitch.Helix.Models.Polls.GetPolls;

public record GetPollsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<Poll> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
