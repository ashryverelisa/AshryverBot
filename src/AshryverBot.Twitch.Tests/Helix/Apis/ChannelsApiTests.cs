using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Channels.GetAdSchedule;
using AshryverBot.Twitch.Helix.Models.Channels.GetChannelEditors;
using AshryverBot.Twitch.Helix.Models.Channels.GetChannelFollowers;
using AshryverBot.Twitch.Helix.Models.Channels.GetChannelInformation;
using AshryverBot.Twitch.Helix.Models.Channels.GetFollowedChannels;
using AshryverBot.Twitch.Helix.Models.Channels.GetVIPs;
using AshryverBot.Twitch.Helix.Models.Channels.ModifyChannelInformation;
using AshryverBot.Twitch.Helix.Models.Channels.Snooze;
using AshryverBot.Twitch.Helix.Models.Channels.StartCommercial;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class ChannelsApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly ChannelsApi _api;
    public ChannelsApiTests() => _api = new ChannelsApi(_client);

    [Fact]
    public async Task GetChannelInformationAsync_passes_multi_broadcaster_ids()
    {
        _client.GetAsync<GetChannelInformationResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetChannelInformationResponse([]));

        await _api.GetChannelInformationAsync("tok", ["bc1", "bc2", "bc3"]);

        await _client.Received(1).GetAsync<GetChannelInformationResponse>(
            "channels", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.CountKey("broadcaster_id") == 3),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ModifyChannelInformationAsync_uses_PATCH_with_body()
    {
        var body = new ModifyChannelInformationRequest { Title = "new" };
        await _api.ModifyChannelInformationAsync("tok", "bc", body);

        await _client.Received(1).PatchAsync(
            "channels", "tok", body,
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetChannelEditorsAsync_calls_channels_editors()
    {
        _client.GetAsync<GetChannelEditorsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetChannelEditorsResponse([]));

        await _api.GetChannelEditorsAsync("tok", "bc");

        await _client.Received(1).GetAsync<GetChannelEditorsResponse>(
            "channels/editors", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetFollowedChannelsAsync_required_only()
    {
        _client.GetAsync<GetFollowedChannelsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetFollowedChannelsResponse([], null, 0));

        await _api.GetFollowedChannelsAsync("tok", "u");

        await _client.Received(1).GetAsync<GetFollowedChannelsResponse>(
            "channels/followed", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("user_id", "u") && q.HasNoKey("broadcaster_id")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetFollowedChannelsAsync_with_broadcaster_filter_and_paging()
    {
        _client.GetAsync<GetFollowedChannelsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetFollowedChannelsResponse([], null, 0));

        await _api.GetFollowedChannelsAsync("tok", "u", "bc", 10, "cur");

        await _client.Received(1).GetAsync<GetFollowedChannelsResponse>(
            "channels/followed", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("user_id", "u") && q.Has("broadcaster_id", "bc")
                && q.Has("first", "10") && q.Has("after", "cur")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetChannelFollowersAsync_required_only()
    {
        _client.GetAsync<GetChannelFollowersResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetChannelFollowersResponse([], null, 0));

        await _api.GetChannelFollowersAsync("tok", "bc");

        await _client.Received(1).GetAsync<GetChannelFollowersResponse>(
            "channels/followers", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.HasNoKey("user_id")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task StartCommercialAsync_uses_POST_commercial_with_body()
    {
        _client.PostAsync<StartCommercialRequest, StartCommercialResponse>(
            null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new StartCommercialResponse(60, "ok", 0));

        var body = new StartCommercialRequest { BroadcasterId = "bc", Length = 60 };
        await _api.StartCommercialAsync("tok", body);

        await _client.Received(1).PostAsync<StartCommercialRequest, StartCommercialResponse>(
            "channels/commercial", "tok", body, null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetAdScheduleAsync_calls_channels_ads()
    {
        _client.GetAsync<GetAdScheduleResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetAdScheduleResponse([]));

        await _api.GetAdScheduleAsync("tok", "bc");

        await _client.Received(1).GetAsync<GetAdScheduleResponse>(
            "channels/ads", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SnoozeNextAdAsync_uses_POST_snooze()
    {
        _client.PostAsync<SnoozeResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new SnoozeResponse([]));

        await _api.SnoozeNextAdAsync("tok", "bc");

        await _client.Received(1).PostAsync<SnoozeResponse>(
            "channels/ads/schedule/snooze", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetVIPsAsync_required_only()
    {
        _client.GetAsync<GetVIPsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetVIPsResponse([], null));

        await _api.GetVIPsAsync("tok", "bc");

        await _client.Received(1).GetAsync<GetVIPsResponse>(
            "channels/vips", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.HasNoKey("user_id")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetVIPsAsync_multi_user_filter()
    {
        _client.GetAsync<GetVIPsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetVIPsResponse([], null));

        await _api.GetVIPsAsync("tok", "bc", ["u1", "u2"], 25, "cur");

        await _client.Received(1).GetAsync<GetVIPsResponse>(
            "channels/vips", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.CountKey("user_id") == 2
                && q.Has("first", "25") && q.Has("after", "cur")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AddChannelVipAsync_uses_POST_vips()
    {
        await _api.AddChannelVipAsync("tok", "u", "bc");

        await _client.Received(1).PostAsync(
            "channels/vips", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("user_id", "u") && q.Has("broadcaster_id", "bc")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RemoveChannelVipAsync_uses_DELETE_vips()
    {
        await _api.RemoveChannelVipAsync("tok", "u", "bc");

        await _client.Received(1).DeleteAsync(
            "channels/vips", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("user_id", "u") && q.Has("broadcaster_id", "bc")),
            Arg.Any<CancellationToken>());
    }
}
