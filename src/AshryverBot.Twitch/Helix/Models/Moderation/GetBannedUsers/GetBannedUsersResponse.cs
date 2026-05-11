using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;
using AshryverBot.Twitch.Helix.Models.Moderation.Common;

namespace AshryverBot.Twitch.Helix.Models.Moderation.GetBannedUsers;

public record GetBannedUsersResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<BannedUser> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
