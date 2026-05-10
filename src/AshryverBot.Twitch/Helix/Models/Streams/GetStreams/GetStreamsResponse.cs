using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;
using AshryverBot.Twitch.Helix.Models.Streams.Common;

namespace AshryverBot.Twitch.Helix.Models.Streams.GetStreams;

public record GetStreamsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<LiveStream> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
