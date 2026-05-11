using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Streams.CreateStreamMarker;
using AshryverBot.Twitch.Helix.Models.Streams.GetFollowedStreams;
using AshryverBot.Twitch.Helix.Models.Streams.GetStreamKey;
using AshryverBot.Twitch.Helix.Models.Streams.GetStreamMarkers;
using AshryverBot.Twitch.Helix.Models.Streams.GetStreams;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class StreamsApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly StreamsApi _api;
    public StreamsApiTests() => _api = new StreamsApi(_client);

    [Fact]
    public async Task GetStreamKeyAsync_calls_streams_key()
    {
        _client.GetAsync<GetStreamKeyResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetStreamKeyResponse([]));

        await _api.GetStreamKeyAsync("tok", "bc");

        await _client.Received(1).GetAsync<GetStreamKeyResponse>(
            "streams/key", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetStreamsAsync_no_params()
    {
        _client.GetAsync<GetStreamsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetStreamsResponse([], null));

        await _api.GetStreamsAsync("tok");

        await _client.Received(1).GetAsync<GetStreamsResponse>(
            "streams", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.TotalCount() == 0),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetStreamsAsync_all_filters_multi_values()
    {
        _client.GetAsync<GetStreamsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetStreamsResponse([], null));

        await _api.GetStreamsAsync("tok",
            userIds: ["u1", "u2"], userLogins: ["a"],
            gameIds: ["g1", "g2", "g3"], type: "live",
            languages: ["en", "de"], first: 50, before: "b", after: "a2");

        await _client.Received(1).GetAsync<GetStreamsResponse>(
            "streams", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.CountKey("user_id") == 2 && q.CountKey("user_login") == 1
                && q.CountKey("game_id") == 3 && q.Has("type", "live")
                && q.CountKey("language") == 2 && q.Has("first", "50")
                && q.Has("before", "b") && q.Has("after", "a2")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetFollowedStreamsAsync_required_and_paging()
    {
        _client.GetAsync<GetFollowedStreamsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetFollowedStreamsResponse([], null));

        await _api.GetFollowedStreamsAsync("tok", "u", 10, "cur");

        await _client.Received(1).GetAsync<GetFollowedStreamsResponse>(
            "streams/followed", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("user_id", "u") && q.Has("first", "10") && q.Has("after", "cur")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateStreamMarkerAsync_forwards_body()
    {
        _client.PostAsync<CreateStreamMarkerRequest, CreateStreamMarkerResponse>(null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new CreateStreamMarkerResponse([]));

        var body = new CreateStreamMarkerRequest("u", "great moment");
        await _api.CreateStreamMarkerAsync("tok", body);

        await _client.Received(1).PostAsync<CreateStreamMarkerRequest, CreateStreamMarkerResponse>(
            "streams/markers", "tok", body, null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetStreamMarkersAsync_no_params()
    {
        _client.GetAsync<GetStreamMarkersResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetStreamMarkersResponse([], null));

        await _api.GetStreamMarkersAsync("tok");

        await _client.Received(1).GetAsync<GetStreamMarkersResponse>(
            "streams/markers", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.TotalCount() == 0),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetStreamMarkersAsync_all_params()
    {
        _client.GetAsync<GetStreamMarkersResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetStreamMarkersResponse([], null));

        await _api.GetStreamMarkersAsync("tok", "u", "v", "after", "before", 25);

        await _client.Received(1).GetAsync<GetStreamMarkersResponse>(
            "streams/markers", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("user_id", "u") && q.Has("video_id", "v")
                && q.Has("after", "after") && q.Has("before", "before") && q.Has("first", "25")),
            Arg.Any<CancellationToken>());
    }
}
