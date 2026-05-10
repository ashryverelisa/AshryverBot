using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Entitlements.UpdateDropsEntitlements;

public record UpdateDropsEntitlementsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<DropsEntitlementUpdateResult> Data
);

public record DropsEntitlementUpdateResult(
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("ids")] IReadOnlyCollection<string> Ids
);
