using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Channels.Snooze;

public record SnoozeResponse([property: JsonPropertyName("data")] Snooze[] Data);