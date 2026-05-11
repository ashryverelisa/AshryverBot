using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.ChannelPoints.CreateCustomReward;
using AshryverBot.Twitch.Helix.Models.ChannelPoints.GetCustomReward;
using AshryverBot.Twitch.Helix.Models.ChannelPoints.GetCustomRewardRedemption;
using AshryverBot.Twitch.Helix.Models.ChannelPoints.UpdateCustomReward;
using AshryverBot.Twitch.Helix.Models.ChannelPoints.UpdateRedemptionStatus;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class ChannelPointsApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly ChannelPointsApi _api;
    public ChannelPointsApiTests() => _api = new ChannelPointsApi(_client);

    [Fact]
    public async Task CreateCustomRewardAsync_uses_POST_with_broadcaster_id()
    {
        _client.PostAsync<CreateCustomRewardRequest, CreateCustomRewardResponse>(
            null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new CreateCustomRewardResponse([]));

        var body = new CreateCustomRewardRequest { Title = "T", Cost = 100 };
        await _api.CreateCustomRewardAsync("tok", "bc", body);

        await _client.Received(1).PostAsync<CreateCustomRewardRequest, CreateCustomRewardResponse>(
            "channel_points/custom_rewards", "tok", body,
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteCustomRewardAsync_uses_DELETE_with_broadcaster_and_id()
    {
        await _api.DeleteCustomRewardAsync("tok", "bc", "r");

        await _client.Received(1).DeleteAsync(
            "channel_points/custom_rewards", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("id", "r")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetCustomRewardAsync_required_only()
    {
        _client.GetAsync<GetCustomRewardResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetCustomRewardResponse([]));

        await _api.GetCustomRewardAsync("tok", "bc");

        await _client.Received(1).GetAsync<GetCustomRewardResponse>(
            "channel_points/custom_rewards", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetCustomRewardAsync_with_ids_and_manageable_flag()
    {
        _client.GetAsync<GetCustomRewardResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetCustomRewardResponse([]));

        await _api.GetCustomRewardAsync("tok", "bc",
            ids: ["r1", "r2"], onlyManageableRewards: true);

        await _client.Received(1).GetAsync<GetCustomRewardResponse>(
            "channel_points/custom_rewards", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.CountKey("id") == 2
                && q.Has("only_manageable_rewards", "true")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetCustomRewardRedemptionAsync_required_only()
    {
        _client.GetAsync<GetCustomRewardRedemptionResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetCustomRewardRedemptionResponse([], null));

        await _api.GetCustomRewardRedemptionAsync("tok", "bc", "r");

        await _client.Received(1).GetAsync<GetCustomRewardRedemptionResponse>(
            "channel_points/custom_rewards/redemptions", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("reward_id", "r") && q.TotalCount() == 2),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetCustomRewardRedemptionAsync_all_filters()
    {
        _client.GetAsync<GetCustomRewardRedemptionResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetCustomRewardRedemptionResponse([], null));

        await _api.GetCustomRewardRedemptionAsync("tok", "bc", "r",
            status: "UNFULFILLED", ids: ["x"], sort: "OLDEST", after: "cur", first: 20);

        await _client.Received(1).GetAsync<GetCustomRewardRedemptionResponse>(
            "channel_points/custom_rewards/redemptions", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("status", "UNFULFILLED") && q.Has("id", "x")
                && q.Has("sort", "OLDEST") && q.Has("after", "cur") && q.Has("first", "20")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateCustomRewardAsync_uses_PATCH_with_broadcaster_and_id()
    {
        _client.PatchAsync<UpdateCustomRewardRequest, UpdateCustomRewardResponse>(
            null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new UpdateCustomRewardResponse([]));

        var body = new UpdateCustomRewardRequest { IsPaused = true };
        await _api.UpdateCustomRewardAsync("tok", "bc", "r", body);

        await _client.Received(1).PatchAsync<UpdateCustomRewardRequest, UpdateCustomRewardResponse>(
            "channel_points/custom_rewards", "tok", body,
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("id", "r")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateRedemptionStatusAsync_uses_PATCH_redemptions()
    {
        _client.PatchAsync<UpdateRedemptionStatusRequest, UpdateRedemptionStatusResponse>(
            null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new UpdateRedemptionStatusResponse([]));

        var body = new UpdateRedemptionStatusRequest("FULFILLED");
        await _api.UpdateRedemptionStatusAsync("tok", "bc", "r", ["rd1", "rd2"], body);

        await _client.Received(1).PatchAsync<UpdateRedemptionStatusRequest, UpdateRedemptionStatusResponse>(
            "channel_points/custom_rewards/redemptions", "tok", body,
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("reward_id", "r")
                && q.CountKey("id") == 2),
            Arg.Any<CancellationToken>());
    }
}
