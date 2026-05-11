using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Analytics.GetExtensionAnalytics;
using AshryverBot.Twitch.Helix.Models.Analytics.GetGameAnalytics;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class AnalyticsApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly AnalyticsApi _api;
    public AnalyticsApiTests() => _api = new AnalyticsApi(_client);

    [Fact]
    public async Task GetExtensionAnalyticsAsync_defaults_send_empty_query()
    {
        var expected = new GetExtensionAnalyticsResponse([], null);
        _client.GetAsync<GetExtensionAnalyticsResponse>("analytics/extensions", "tok",
            Arg.Any<IReadOnlyList<KeyValuePair<string, string>>>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        var result = await _api.GetExtensionAnalyticsAsync("tok");

        Assert.Same(expected, result);
        await _client.Received(1).GetAsync<GetExtensionAnalyticsResponse>(
            "analytics/extensions", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.TotalCount() == 0),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetExtensionAnalyticsAsync_all_params_forwarded()
    {
        _client.GetAsync<GetExtensionAnalyticsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetExtensionAnalyticsResponse([], null));

        var start = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var end = new DateTimeOffset(2026, 5, 1, 0, 0, 0, TimeSpan.Zero);
        await _api.GetExtensionAnalyticsAsync("tok", "ext", "overview_v2", start, end, 10, "cur");

        await _client.Received(1).GetAsync<GetExtensionAnalyticsResponse>(
            "analytics/extensions", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("extension_id", "ext")
                && q.Has("type", "overview_v2")
                && q.CountKey("started_at") == 1
                && q.CountKey("ended_at") == 1
                && q.Has("first", "10")
                && q.Has("after", "cur")
                && q.TotalCount() == 6),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetGameAnalyticsAsync_all_params_forwarded()
    {
        _client.GetAsync<GetGameAnalyticsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetGameAnalyticsResponse([], null));

        await _api.GetGameAnalyticsAsync("tok", "g", "overview_v2", null, null, 5, "cur");

        await _client.Received(1).GetAsync<GetGameAnalyticsResponse>(
            "analytics/games", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("game_id", "g") && q.Has("type", "overview_v2")
                && q.Has("first", "5") && q.Has("after", "cur")
                && q.HasNoKey("started_at") && q.HasNoKey("ended_at")),
            Arg.Any<CancellationToken>());
    }
}
