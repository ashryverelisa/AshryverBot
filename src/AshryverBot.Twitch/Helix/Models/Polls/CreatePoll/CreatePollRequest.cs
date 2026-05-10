using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Polls.CreatePoll;

public record CreatePollRequest
{
    [JsonPropertyName("broadcaster_id")]
    public string BroadcasterId { get; init; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; init; } = string.Empty;

    [JsonPropertyName("choices")]
    public IReadOnlyCollection<CreatePollChoice> Choices { get; init; } = [];

    [JsonPropertyName("duration")]
    public int Duration { get; init; }

    [JsonPropertyName("channel_points_voting_enabled")]
    public bool? ChannelPointsVotingEnabled { get; init; }

    [JsonPropertyName("channel_points_per_vote")]
    public int? ChannelPointsPerVote { get; init; }
}

public record CreatePollChoice(
    [property: JsonPropertyName("title")] string Title
);
