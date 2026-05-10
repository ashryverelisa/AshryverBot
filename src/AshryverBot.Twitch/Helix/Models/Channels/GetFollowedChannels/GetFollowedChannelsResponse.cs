using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Channels.GetFollowedChannels;

public record GetFollowedChannelsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<FollowedChannel> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination,
    [property: JsonPropertyName("total")] int Total
);
