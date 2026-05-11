using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Moderation.GetUnbanRequests;

public record GetUnbanRequestsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<UnbanRequest> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
