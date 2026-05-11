using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Schedule.Common;
using AshryverBot.Twitch.Helix.Models.Schedule.CreateChannelStreamScheduleSegment;
using AshryverBot.Twitch.Helix.Models.Schedule.GetChannelStreamSchedule;
using AshryverBot.Twitch.Helix.Models.Schedule.UpdateChannelStreamSchedule;
using AshryverBot.Twitch.Helix.Models.Schedule.UpdateChannelStreamScheduleSegment;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class ScheduleApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly ScheduleApi _api;
    public ScheduleApiTests() => _api = new ScheduleApi(_client);

    private static readonly StreamSchedule EmptySchedule =
        new([], "bc", "Name", "name", null);

    [Fact]
    public async Task GetChannelStreamScheduleAsync_required_only()
    {
        _client.GetAsync<GetChannelStreamScheduleResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetChannelStreamScheduleResponse(EmptySchedule, null));

        await _api.GetChannelStreamScheduleAsync("tok", "bc");

        await _client.Received(1).GetAsync<GetChannelStreamScheduleResponse>(
            "schedule", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetChannelStreamScheduleAsync_all_params()
    {
        _client.GetAsync<GetChannelStreamScheduleResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetChannelStreamScheduleResponse(EmptySchedule, null));

        var start = DateTimeOffset.UtcNow;
        await _api.GetChannelStreamScheduleAsync("tok", "bc",
            ["s1", "s2"], start, 25, "cur");

        await _client.Received(1).GetAsync<GetChannelStreamScheduleResponse>(
            "schedule", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.CountKey("id") == 2
                && q.CountKey("start_time") == 1
                && q.Has("first", "25") && q.Has("after", "cur")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateChannelStreamScheduleAsync_uses_PATCH_settings_with_body()
    {
        var body = new UpdateChannelStreamScheduleRequest { IsVacationEnabled = true };
        await _api.UpdateChannelStreamScheduleAsync("tok", "bc", body);

        await _client.Received(1).PatchAsync(
            "schedule/settings", "tok", body,
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateChannelStreamScheduleSegmentAsync_uses_POST_segment_with_body()
    {
        _client.PostAsync<CreateChannelStreamScheduleSegmentRequest, CreateChannelStreamScheduleSegmentResponse>(
            null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new CreateChannelStreamScheduleSegmentResponse(EmptySchedule));

        var body = new CreateChannelStreamScheduleSegmentRequest
        {
            StartTime = DateTimeOffset.UtcNow,
            Timezone = "UTC",
            Duration = "60",
        };
        await _api.CreateChannelStreamScheduleSegmentAsync("tok", "bc", body);

        await _client.Received(1).PostAsync<CreateChannelStreamScheduleSegmentRequest, CreateChannelStreamScheduleSegmentResponse>(
            "schedule/segment", "tok", body,
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.Has("broadcaster_id", "bc")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateChannelStreamScheduleSegmentAsync_uses_PATCH_segment_with_body_and_id()
    {
        _client.PatchAsync<UpdateChannelStreamScheduleSegmentRequest, UpdateChannelStreamScheduleSegmentResponse>(
            null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new UpdateChannelStreamScheduleSegmentResponse(EmptySchedule));

        var body = new UpdateChannelStreamScheduleSegmentRequest { Title = "new" };
        await _api.UpdateChannelStreamScheduleSegmentAsync("tok", "bc", "seg", body);

        await _client.Received(1).PatchAsync<UpdateChannelStreamScheduleSegmentRequest, UpdateChannelStreamScheduleSegmentResponse>(
            "schedule/segment", "tok", body,
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("id", "seg")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteChannelStreamScheduleSegmentAsync_uses_DELETE_segment()
    {
        await _api.DeleteChannelStreamScheduleSegmentAsync("tok", "bc", "seg");

        await _client.Received(1).DeleteAsync(
            "schedule/segment", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("id", "seg") && q.TotalCount() == 2),
            Arg.Any<CancellationToken>());
    }
}
