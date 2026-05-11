using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.Schedule.CreateChannelStreamScheduleSegment;
using AshryverBot.Twitch.Helix.Models.Schedule.GetChannelStreamSchedule;
using AshryverBot.Twitch.Helix.Models.Schedule.UpdateChannelStreamSchedule;
using AshryverBot.Twitch.Helix.Models.Schedule.UpdateChannelStreamScheduleSegment;

namespace AshryverBot.Twitch.Helix.Apis;

public class ScheduleApi(ITwitchClient client) : IScheduleApi
{
    public Task<GetChannelStreamScheduleResponse> GetChannelStreamScheduleAsync(
        string accessToken,
        string broadcasterId,
        IEnumerable<string>? ids = null,
        DateTimeOffset? startTime = null,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        query.AddMany("id", ids);
        query.AddIfNotNull("start_time", startTime);
        query.AddIfNotNull("first", first);
        query.AddIfNotNull("after", after);
        return client.GetAsync<GetChannelStreamScheduleResponse>("schedule", accessToken, query, cancellationToken);
    }

    public Task UpdateChannelStreamScheduleAsync(
        string accessToken,
        string broadcasterId,
        UpdateChannelStreamScheduleRequest body,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        return client.PatchAsync("schedule/settings", accessToken, body, query, cancellationToken);
    }

    public Task<CreateChannelStreamScheduleSegmentResponse> CreateChannelStreamScheduleSegmentAsync(
        string accessToken,
        string broadcasterId,
        CreateChannelStreamScheduleSegmentRequest body,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        return client.PostAsync<CreateChannelStreamScheduleSegmentRequest, CreateChannelStreamScheduleSegmentResponse>(
            "schedule/segment", accessToken, body, query, cancellationToken);
    }

    public Task<UpdateChannelStreamScheduleSegmentResponse> UpdateChannelStreamScheduleSegmentAsync(
        string accessToken,
        string broadcasterId,
        string id,
        UpdateChannelStreamScheduleSegmentRequest body,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("id", id),
        };
        return client.PatchAsync<UpdateChannelStreamScheduleSegmentRequest, UpdateChannelStreamScheduleSegmentResponse>(
            "schedule/segment", accessToken, body, query, cancellationToken);
    }

    public Task DeleteChannelStreamScheduleSegmentAsync(
        string accessToken,
        string broadcasterId,
        string id,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("id", id),
        };
        return client.DeleteAsync("schedule/segment", accessToken, query, cancellationToken);
    }
}
