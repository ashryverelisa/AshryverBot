using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Teams.GetChannelTeams;
using AshryverBot.Twitch.Helix.Models.Teams.GetTeams;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class TeamsApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly TeamsApi _api;
    public TeamsApiTests() => _api = new TeamsApi(_client);

    [Fact]
    public async Task GetChannelTeamsAsync_uses_teams_channel_path()
    {
        _client.GetAsync<GetChannelTeamsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetChannelTeamsResponse([]));

        await _api.GetChannelTeamsAsync("tok", "bc");

        await _client.Received(1).GetAsync<GetChannelTeamsResponse>(
            "teams/channel", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetTeamsAsync_name_only()
    {
        _client.GetAsync<GetTeamsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetTeamsResponse([]));

        await _api.GetTeamsAsync("tok", name: "speedrun");

        await _client.Received(1).GetAsync<GetTeamsResponse>(
            "teams", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("name", "speedrun") && q.HasNoKey("id")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetTeamsAsync_id_only()
    {
        _client.GetAsync<GetTeamsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetTeamsResponse([]));

        await _api.GetTeamsAsync("tok", id: "42");

        await _client.Received(1).GetAsync<GetTeamsResponse>(
            "teams", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("id", "42") && q.HasNoKey("name")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetTeamsAsync_no_params_sends_empty_query()
    {
        _client.GetAsync<GetTeamsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetTeamsResponse([]));

        await _api.GetTeamsAsync("tok");

        await _client.Received(1).GetAsync<GetTeamsResponse>(
            "teams", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.TotalCount() == 0),
            Arg.Any<CancellationToken>());
    }
}
