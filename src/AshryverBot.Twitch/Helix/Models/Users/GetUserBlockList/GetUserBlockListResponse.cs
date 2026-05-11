using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Users.GetUserBlockList;

public record GetUserBlockListResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<BlockedUser> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
