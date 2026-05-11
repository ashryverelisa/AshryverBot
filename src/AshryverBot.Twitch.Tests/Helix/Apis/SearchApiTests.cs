using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Search.SearchCategories;
using AshryverBot.Twitch.Helix.Models.Search.SearchChannels;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class SearchApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly SearchApi _api;
    public SearchApiTests() => _api = new SearchApi(_client);

    [Fact]
    public async Task SearchCategoriesAsync_required_only()
    {
        _client.GetAsync<SearchCategoriesResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new SearchCategoriesResponse([], null));

        await _api.SearchCategoriesAsync("tok", "rocket");

        await _client.Received(1).GetAsync<SearchCategoriesResponse>(
            "search/categories", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("query", "rocket") && q.HasNoKey("first") && q.HasNoKey("after")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SearchCategoriesAsync_with_paging()
    {
        _client.GetAsync<SearchCategoriesResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new SearchCategoriesResponse([], null));

        await _api.SearchCategoriesAsync("tok", "rocket", 25, "cur");

        await _client.Received(1).GetAsync<SearchCategoriesResponse>(
            "search/categories", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("query", "rocket") && q.Has("first", "25") && q.Has("after", "cur")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SearchChannelsAsync_required_only()
    {
        _client.GetAsync<SearchChannelsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new SearchChannelsResponse([], null));

        await _api.SearchChannelsAsync("tok", "anna");

        await _client.Received(1).GetAsync<SearchChannelsResponse>(
            "search/channels", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("query", "anna") && q.HasNoKey("live_only")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SearchChannelsAsync_live_only_true_emits_lowercase()
    {
        _client.GetAsync<SearchChannelsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new SearchChannelsResponse([], null));

        await _api.SearchChannelsAsync("tok", "anna", liveOnly: true, first: 10, after: "cur");

        await _client.Received(1).GetAsync<SearchChannelsResponse>(
            "search/channels", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("query", "anna") && q.Has("live_only", "true")
                && q.Has("first", "10") && q.Has("after", "cur")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SearchChannelsAsync_live_only_false_emits_false()
    {
        _client.GetAsync<SearchChannelsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new SearchChannelsResponse([], null));

        await _api.SearchChannelsAsync("tok", "x", liveOnly: false);

        await _client.Received(1).GetAsync<SearchChannelsResponse>(
            "search/channels", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.Has("live_only", "false")),
            Arg.Any<CancellationToken>());
    }
}
