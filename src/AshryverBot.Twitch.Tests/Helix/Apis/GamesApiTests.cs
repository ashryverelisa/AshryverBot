using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Games.GetGames;
using AshryverBot.Twitch.Helix.Models.Games.GetTopGames;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class GamesApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly GamesApi _api;
    public GamesApiTests() => _api = new GamesApi(_client);

    [Fact]
    public async Task GetTopGamesAsync_defaults_send_empty_query()
    {
        _client.GetAsync<GetTopGamesResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetTopGamesResponse([], null));

        await _api.GetTopGamesAsync("tok");

        await _client.Received(1).GetAsync<GetTopGamesResponse>(
            "games/top", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.TotalCount() == 0),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetTopGamesAsync_all_paging_params()
    {
        _client.GetAsync<GetTopGamesResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetTopGamesResponse([], null));

        await _api.GetTopGamesAsync("tok", 20, "a", "b");

        await _client.Received(1).GetAsync<GetTopGamesResponse>(
            "games/top", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("first", "20") && q.Has("after", "a") && q.Has("before", "b")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetGamesAsync_ids_names_igdb_each_multi()
    {
        _client.GetAsync<GetGamesResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetGamesResponse([]));

        await _api.GetGamesAsync("tok",
            ids: ["1", "2"],
            names: ["Halo"],
            igdbIds: ["100", "101", "102"]);

        await _client.Received(1).GetAsync<GetGamesResponse>(
            "games", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.CountKey("id") == 2 && q.Has("id", "1") && q.Has("id", "2")
                && q.CountKey("name") == 1 && q.Has("name", "Halo")
                && q.CountKey("igdb_id") == 3),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetGamesAsync_no_params_sends_empty_query()
    {
        _client.GetAsync<GetGamesResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetGamesResponse([]));

        await _api.GetGamesAsync("tok");

        await _client.Received(1).GetAsync<GetGamesResponse>(
            "games", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.TotalCount() == 0),
            Arg.Any<CancellationToken>());
    }
}
