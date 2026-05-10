using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;
using AshryverBot.Twitch.Helix.Models.Schedule.Common;

namespace AshryverBot.Twitch.Helix.Models.Schedule.GetChannelStreamSchedule;

public record GetChannelStreamScheduleResponse(
    [property: JsonPropertyName("data")] StreamSchedule Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
