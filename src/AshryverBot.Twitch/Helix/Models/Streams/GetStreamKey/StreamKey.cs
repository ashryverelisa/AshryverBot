using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Streams.GetStreamKey;

public record StreamKey(
    [property: JsonPropertyName("stream_key")] string Key
);
