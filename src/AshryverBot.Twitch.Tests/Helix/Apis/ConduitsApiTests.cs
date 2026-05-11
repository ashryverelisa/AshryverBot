using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Conduits.CreateConduits;
using AshryverBot.Twitch.Helix.Models.Conduits.GetConduits;
using AshryverBot.Twitch.Helix.Models.Conduits.GetConduitShards;
using AshryverBot.Twitch.Helix.Models.Conduits.UpdateConduits;
using AshryverBot.Twitch.Helix.Models.Conduits.UpdateConduitShards;
using AshryverBot.Twitch.Helix.Models.EventSub.Common;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class ConduitsApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly ConduitsApi _api;
    public ConduitsApiTests() => _api = new ConduitsApi(_client);

    [Fact]
    public async Task GetConduitsAsync_uses_GET_no_query()
    {
        _client.GetAsync<GetConduitsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetConduitsResponse([]));

        await _api.GetConduitsAsync("tok");

        await _client.Received(1).GetAsync<GetConduitsResponse>(
            "eventsub/conduits", "tok", null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateConduitsAsync_forwards_body()
    {
        _client.PostAsync<CreateConduitsRequest, CreateConduitsResponse>(null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new CreateConduitsResponse([]));

        var body = new CreateConduitsRequest(3);
        await _api.CreateConduitsAsync("tok", body);

        await _client.Received(1).PostAsync<CreateConduitsRequest, CreateConduitsResponse>(
            "eventsub/conduits", "tok", body, null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateConduitsAsync_uses_PATCH_with_body()
    {
        _client.PatchAsync<UpdateConduitsRequest, UpdateConduitsResponse>(null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new UpdateConduitsResponse([]));

        var body = new UpdateConduitsRequest("c", 4);
        await _api.UpdateConduitsAsync("tok", body);

        await _client.Received(1).PatchAsync<UpdateConduitsRequest, UpdateConduitsResponse>(
            "eventsub/conduits", "tok", body, null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteConduitAsync_uses_DELETE_with_id_query()
    {
        await _api.DeleteConduitAsync("tok", "c");

        await _client.Received(1).DeleteAsync(
            "eventsub/conduits", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("id", "c") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetConduitShardsAsync_required_only()
    {
        _client.GetAsync<GetConduitShardsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetConduitShardsResponse([], null));

        await _api.GetConduitShardsAsync("tok", "c");

        await _client.Received(1).GetAsync<GetConduitShardsResponse>(
            "eventsub/conduits/shards", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("conduit_id", "c") && q.HasNoKey("status") && q.HasNoKey("after")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetConduitShardsAsync_with_status_and_after()
    {
        _client.GetAsync<GetConduitShardsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetConduitShardsResponse([], null));

        await _api.GetConduitShardsAsync("tok", "c", "enabled", "cur");

        await _client.Received(1).GetAsync<GetConduitShardsResponse>(
            "eventsub/conduits/shards", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("conduit_id", "c") && q.Has("status", "enabled") && q.Has("after", "cur")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateConduitShardsAsync_uses_PATCH_with_body()
    {
        _client.PatchAsync<UpdateConduitShardsRequest, UpdateConduitShardsResponse>(
            null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new UpdateConduitShardsResponse([], []));

        var transport = new EventSubTransport("webhook", "https://cb", "secret", null, null, null, null);
        var body = new UpdateConduitShardsRequest("c", [new UpdateConduitShard("0", transport)]);
        await _api.UpdateConduitShardsAsync("tok", body);

        await _client.Received(1).PatchAsync<UpdateConduitShardsRequest, UpdateConduitShardsResponse>(
            "eventsub/conduits/shards", "tok", body, null, Arg.Any<CancellationToken>());
    }
}
