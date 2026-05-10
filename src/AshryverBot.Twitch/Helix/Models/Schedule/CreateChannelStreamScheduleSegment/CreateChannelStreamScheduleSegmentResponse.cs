using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Schedule.Common;

namespace AshryverBot.Twitch.Helix.Models.Schedule.CreateChannelStreamScheduleSegment;

public record CreateChannelStreamScheduleSegmentResponse(
    [property: JsonPropertyName("data")] StreamSchedule Data
);
