using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.EventSub.Common;

public record EventSubTransport(
    [property: JsonPropertyName("method")] string Method,
    [property: JsonPropertyName("callback")] string? Callback,
    [property: JsonPropertyName("secret")] string? Secret,
    [property: JsonPropertyName("session_id")] string? SessionId,
    [property: JsonPropertyName("conduit_id")] string? ConduitId,
    [property: JsonPropertyName("connected_at")] DateTimeOffset? ConnectedAt,
    [property: JsonPropertyName("disconnected_at")] DateTimeOffset? DisconnectedAt
);
