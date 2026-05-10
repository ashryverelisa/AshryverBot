using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.ChannelPoints.UpdateRedemptionStatus;

public record UpdateRedemptionStatusRequest(
    [property: JsonPropertyName("status")] string Status
);
