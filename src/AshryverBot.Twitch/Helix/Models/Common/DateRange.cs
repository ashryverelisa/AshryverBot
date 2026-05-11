using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Common;

public record DateRange(
    [property: JsonPropertyName("started_at")] DateTimeOffset StartedAt,
    [property: JsonPropertyName("ended_at")] DateTimeOffset EndedAt
);
