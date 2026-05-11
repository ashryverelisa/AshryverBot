using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Entitlements.GetDropsEntitlements;

public record DropsEntitlement(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("benefit_id")] string BenefitId,
    [property: JsonPropertyName("timestamp")] DateTimeOffset Timestamp,
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("game_id")] string GameId,
    [property: JsonPropertyName("fulfillment_status")] string FulfillmentStatus,
    [property: JsonPropertyName("last_updated")] DateTimeOffset LastUpdated
);
