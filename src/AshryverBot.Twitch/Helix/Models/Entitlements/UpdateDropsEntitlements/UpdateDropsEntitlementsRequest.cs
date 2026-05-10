using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Entitlements.UpdateDropsEntitlements;

public record UpdateDropsEntitlementsRequest(
    [property: JsonPropertyName("entitlement_ids")] IReadOnlyCollection<string> EntitlementIds,
    [property: JsonPropertyName("fulfillment_status")] string FulfillmentStatus
);
