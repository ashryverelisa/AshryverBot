using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;
using AshryverBot.Twitch.Helix.Models.Streams.Common;

namespace AshryverBot.Twitch.Helix.Models.Streams.GetFollowedStreams;

public record GetFollowedStreamsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<LiveStream> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
