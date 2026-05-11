using AshryverBot.Twitch.Helix.Models.Schedule.CreateChannelStreamScheduleSegment;
using AshryverBot.Twitch.Helix.Models.Schedule.GetChannelStreamSchedule;
using AshryverBot.Twitch.Helix.Models.Schedule.UpdateChannelStreamSchedule;
using AshryverBot.Twitch.Helix.Models.Schedule.UpdateChannelStreamScheduleSegment;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IScheduleApi
{
    Task<GetChannelStreamScheduleResponse> GetChannelStreamScheduleAsync(
        string accessToken,
        string broadcasterId,
        IEnumerable<string>? ids = null,
        DateTimeOffset? startTime = null,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default);

    Task UpdateChannelStreamScheduleAsync(
        string accessToken,
        string broadcasterId,
        UpdateChannelStreamScheduleRequest body,
        CancellationToken cancellationToken = default);

    Task<CreateChannelStreamScheduleSegmentResponse> CreateChannelStreamScheduleSegmentAsync(
        string accessToken,
        string broadcasterId,
        CreateChannelStreamScheduleSegmentRequest body,
        CancellationToken cancellationToken = default);

    Task<UpdateChannelStreamScheduleSegmentResponse> UpdateChannelStreamScheduleSegmentAsync(
        string accessToken,
        string broadcasterId,
        string id,
        UpdateChannelStreamScheduleSegmentRequest body,
        CancellationToken cancellationToken = default);

    Task DeleteChannelStreamScheduleSegmentAsync(
        string accessToken,
        string broadcasterId,
        string id,
        CancellationToken cancellationToken = default);
}
