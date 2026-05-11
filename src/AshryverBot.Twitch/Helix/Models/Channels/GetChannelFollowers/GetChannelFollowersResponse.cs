using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Channels.GetChannelFollowers;

public record GetChannelFollowersResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ChannelFollower> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination,
    [property: JsonPropertyName("total")] int Total
);
