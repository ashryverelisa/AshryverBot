using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Videos.DeleteVideos;
using AshryverBot.Twitch.Helix.Models.Videos.GetVideos;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class VideosApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly VideosApi _api;
    public VideosApiTests() => _api = new VideosApi(_client);

    [Fact]
    public async Task GetVideosAsync_no_params_sends_empty_query()
    {
        _client.GetAsync<GetVideosResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetVideosResponse([], null));

        await _api.GetVideosAsync("tok");

        await _client.Received(1).GetAsync<GetVideosResponse>(
            "videos", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.TotalCount() == 0),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetVideosAsync_all_filters()
    {
        _client.GetAsync<GetVideosResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetVideosResponse([], null));

        await _api.GetVideosAsync("tok",
            ids: ["v1", "v2"], userId: "u", gameId: "g",
            language: "en", period: "month", sort: "time", type: "archive",
            first: 10, after: "a", before: "b");

        await _client.Received(1).GetAsync<GetVideosResponse>(
            "videos", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.CountKey("id") == 2 && q.Has("user_id", "u") && q.Has("game_id", "g")
                && q.Has("language", "en") && q.Has("period", "month") && q.Has("sort", "time")
                && q.Has("type", "archive") && q.Has("first", "10")
                && q.Has("after", "a") && q.Has("before", "b")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteVideosAsync_calls_DELETE_videos_with_ids()
    {
        _client.DeleteAsync<DeleteVideosResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new DeleteVideosResponse([]));

        await _api.DeleteVideosAsync("tok", ["v1", "v2", "v3"]);

        await _client.Received(1).DeleteAsync<DeleteVideosResponse>(
            "videos", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.CountKey("id") == 3 && q.Has("id", "v1") && q.Has("id", "v2") && q.Has("id", "v3")),
            Arg.Any<CancellationToken>());
    }
}
