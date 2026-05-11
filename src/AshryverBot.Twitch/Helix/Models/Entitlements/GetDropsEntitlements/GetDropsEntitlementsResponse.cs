using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Entitlements.GetDropsEntitlements;

public record GetDropsEntitlementsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<DropsEntitlement> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
