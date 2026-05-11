using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Bits.GetBitsLeaderboard;
using AshryverBot.Twitch.Helix.Models.Bits.GetCheermotes;
using AshryverBot.Twitch.Helix.Models.Bits.GetExtensionTransactions;
using AshryverBot.Twitch.Helix.Models.Common;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class BitsApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly BitsApi _api;
    public BitsApiTests() => _api = new BitsApi(_client);

    [Fact]
    public async Task GetBitsLeaderboardAsync_defaults()
    {
        _client.GetAsync<GetBitsLeaderboardResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetBitsLeaderboardResponse(
                [],
                new DateRange(DateTimeOffset.MinValue, DateTimeOffset.MaxValue), 0));

        await _api.GetBitsLeaderboardAsync("tok");

        await _client.Received(1).GetAsync<GetBitsLeaderboardResponse>(
            "bits/leaderboard", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.TotalCount() == 0),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetBitsLeaderboardAsync_all_params_forwarded()
    {
        _client.GetAsync<GetBitsLeaderboardResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetBitsLeaderboardResponse(
                [],
                new DateRange(DateTimeOffset.MinValue, DateTimeOffset.MaxValue), 0));

        await _api.GetBitsLeaderboardAsync("tok", 5, "day", DateTimeOffset.UtcNow, "u1");

        await _client.Received(1).GetAsync<GetBitsLeaderboardResponse>(
            "bits/leaderboard", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("count", "5") && q.Has("period", "day")
                && q.CountKey("started_at") == 1 && q.Has("user_id", "u1")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetCheermotesAsync_omits_optional_when_null()
    {
        _client.GetAsync<GetCheermotesResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetCheermotesResponse([]));

        await _api.GetCheermotesAsync("tok");

        await _client.Received(1).GetAsync<GetCheermotesResponse>(
            "bits/cheermotes", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.HasNoKey("broadcaster_id")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetCheermotesAsync_with_broadcaster_id()
    {
        _client.GetAsync<GetCheermotesResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetCheermotesResponse([]));

        await _api.GetCheermotesAsync("tok", "bc");

        await _client.Received(1).GetAsync<GetCheermotesResponse>(
            "bits/cheermotes", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.Has("broadcaster_id", "bc")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetExtensionTransactionsAsync_required_extension_id()
    {
        _client.GetAsync<GetExtensionTransactionsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetExtensionTransactionsResponse([], null));

        await _api.GetExtensionTransactionsAsync("tok", "ext");

        await _client.Received(1).GetAsync<GetExtensionTransactionsResponse>(
            "extensions/transactions", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("extension_id", "ext") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetExtensionTransactionsAsync_multi_ids_and_paging()
    {
        _client.GetAsync<GetExtensionTransactionsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetExtensionTransactionsResponse([], null));

        await _api.GetExtensionTransactionsAsync("tok", "ext", ["t1", "t2"], 50, "cur");

        await _client.Received(1).GetAsync<GetExtensionTransactionsResponse>(
            "extensions/transactions", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("extension_id", "ext") && q.Has("id", "t1") && q.Has("id", "t2")
                && q.Has("first", "50") && q.Has("after", "cur")),
            Arg.Any<CancellationToken>());
    }
}
