using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Extensions.GetExtensionLiveChannels;

public record GetExtensionLiveChannelsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ExtensionLiveChannel> Data,
    [property: JsonPropertyName("pagination")] string? Pagination
);
