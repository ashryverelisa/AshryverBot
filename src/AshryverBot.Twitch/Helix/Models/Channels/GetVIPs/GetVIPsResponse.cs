using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Channels.GetVIPs;

public record GetVIPsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<Vip> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
