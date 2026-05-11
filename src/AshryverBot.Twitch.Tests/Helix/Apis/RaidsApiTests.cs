using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Raids.StartARaid;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class RaidsApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly RaidsApi _api;
    public RaidsApiTests() => _api = new RaidsApi(_client);

    [Fact]
    public async Task StartARaidAsync_uses_POST_raids_with_from_and_to()
    {
        _client.PostAsync<StartARaidResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new StartARaidResponse([]));

        await _api.StartARaidAsync("tok", "from", "to");

        await _client.Received(1).PostAsync<StartARaidResponse>(
            "raids", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("from_broadcaster_id", "from") && q.Has("to_broadcaster_id", "to")
                && q.TotalCount() == 2),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CancelARaidAsync_uses_DELETE_raids_with_broadcaster_id()
    {
        await _api.CancelARaidAsync("tok", "bc");

        await _client.Received(1).DeleteAsync(
            "raids", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }
}
