using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Schedule.Common;

namespace AshryverBot.Twitch.Helix.Models.Schedule.UpdateChannelStreamScheduleSegment;

public record UpdateChannelStreamScheduleSegmentResponse(
    [property: JsonPropertyName("data")] StreamSchedule Data
);
