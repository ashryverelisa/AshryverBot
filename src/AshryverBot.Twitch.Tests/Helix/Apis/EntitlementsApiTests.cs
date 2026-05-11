using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Entitlements.GetDropsEntitlements;
using AshryverBot.Twitch.Helix.Models.Entitlements.UpdateDropsEntitlements;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class EntitlementsApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly EntitlementsApi _api;
    public EntitlementsApiTests() => _api = new EntitlementsApi(_client);

    [Fact]
    public async Task GetDropsEntitlementsAsync_no_params_sends_empty_query()
    {
        _client.GetAsync<GetDropsEntitlementsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetDropsEntitlementsResponse([], null));

        await _api.GetDropsEntitlementsAsync("tok");

        await _client.Received(1).GetAsync<GetDropsEntitlementsResponse>(
            "entitlements/drops", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.TotalCount() == 0),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetDropsEntitlementsAsync_all_params()
    {
        _client.GetAsync<GetDropsEntitlementsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetDropsEntitlementsResponse([], null));

        await _api.GetDropsEntitlementsAsync("tok",
            ids: ["e1", "e2"], userId: "u", gameId: "g",
            fulfillmentStatus: "CLAIMED", after: "cur", first: 50);

        await _client.Received(1).GetAsync<GetDropsEntitlementsResponse>(
            "entitlements/drops", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.CountKey("id") == 2 && q.Has("user_id", "u") && q.Has("game_id", "g")
                && q.Has("fulfillment_status", "CLAIMED")
                && q.Has("after", "cur") && q.Has("first", "50")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateDropsEntitlementsAsync_uses_PATCH_with_body_no_query()
    {
        _client.PatchAsync<UpdateDropsEntitlementsRequest, UpdateDropsEntitlementsResponse>(
            null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new UpdateDropsEntitlementsResponse([]));

        var body = new UpdateDropsEntitlementsRequest(["e1"], "FULFILLED");
        await _api.UpdateDropsEntitlementsAsync("tok", body);

        await _client.Received(1).PatchAsync<UpdateDropsEntitlementsRequest, UpdateDropsEntitlementsResponse>(
            "entitlements/drops", "tok", body, null, Arg.Any<CancellationToken>());
    }
}
