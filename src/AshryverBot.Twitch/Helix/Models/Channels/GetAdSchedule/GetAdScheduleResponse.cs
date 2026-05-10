using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Channels.GetAdSchedule;

public record GetAdScheduleResponse([property: JsonPropertyName("data")] AdSchedule[] Data);