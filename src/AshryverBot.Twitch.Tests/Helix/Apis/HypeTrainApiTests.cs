using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.HypeTrain.GetHypeTrainEvents;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class HypeTrainApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly HypeTrainApi _api;
    public HypeTrainApiTests() => _api = new HypeTrainApi(_client);

    [Fact]
    public async Task GetHypeTrainEventsAsync_required_only()
    {
        _client.GetAsync<GetHypeTrainEventsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetHypeTrainEventsResponse([], null));

        await _api.GetHypeTrainEventsAsync("tok", "bc");

        await _client.Received(1).GetAsync<GetHypeTrainEventsResponse>(
            "hypetrain/events", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.HasNoKey("first") && q.HasNoKey("id") && q.HasNoKey("after")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetHypeTrainEventsAsync_all_params()
    {
        _client.GetAsync<GetHypeTrainEventsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetHypeTrainEventsResponse([], null));

        await _api.GetHypeTrainEventsAsync("tok", "bc", 10, "ev", "cur");

        await _client.Received(1).GetAsync<GetHypeTrainEventsResponse>(
            "hypetrain/events", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("first", "10")
                && q.Has("id", "ev") && q.Has("after", "cur")),
            Arg.Any<CancellationToken>());
    }
}
