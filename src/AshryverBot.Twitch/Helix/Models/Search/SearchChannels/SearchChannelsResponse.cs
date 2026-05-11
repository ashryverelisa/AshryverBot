using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Search.SearchChannels;

public record SearchChannelsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<SearchedChannel> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
