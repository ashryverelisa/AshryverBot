using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Goals.GetCreatorGoals;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class GoalsApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly GoalsApi _api;
    public GoalsApiTests() => _api = new GoalsApi(_client);

    [Fact]
    public async Task GetCreatorGoalsAsync_calls_goals_with_broadcaster_id()
    {
        var expected = new GetCreatorGoalsResponse([]);
        _client.GetAsync<GetCreatorGoalsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(expected);

        var result = await _api.GetCreatorGoalsAsync("tok", "bc");

        Assert.Same(expected, result);
        await _client.Received(1).GetAsync<GetCreatorGoalsResponse>(
            "goals", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }
}
