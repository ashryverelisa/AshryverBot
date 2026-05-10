using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Polls.Common;

public record Poll(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("broadcaster_name")] string BroadcasterName,
    [property: JsonPropertyName("broadcaster_login")] string BroadcasterLogin,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("choices")] IReadOnlyCollection<PollChoice> Choices,
    [property: JsonPropertyName("channel_points_voting_enabled")] bool ChannelPointsVotingEnabled,
    [property: JsonPropertyName("channel_points_per_vote")] int ChannelPointsPerVote,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("duration")] int Duration,
    [property: JsonPropertyName("started_at")] DateTimeOffset StartedAt,
    [property: JsonPropertyName("ended_at")] DateTimeOffset? EndedAt
);

public record PollChoice(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("votes")] int Votes,
    [property: JsonPropertyName("channel_points_votes")] int ChannelPointsVotes
);
