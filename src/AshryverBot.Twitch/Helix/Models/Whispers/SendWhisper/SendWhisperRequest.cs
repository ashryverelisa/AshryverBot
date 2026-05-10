using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Whispers.SendWhisper;

public record SendWhisperRequest(
    [property: JsonPropertyName("message")] string Message
);
