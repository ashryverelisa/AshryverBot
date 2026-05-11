using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Moderation.GetModeratedChannels;

public record GetModeratedChannelsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ModeratedChannel> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
