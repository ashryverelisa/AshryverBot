using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Channels.StartCommercial;

public record StartCommercialResponse(
    [property: JsonPropertyName("length")] int Length,
    [property: JsonPropertyName("message")] string Message,
    [property: JsonPropertyName("retry_after")] int RetryAfter
);