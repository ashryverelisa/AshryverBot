using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Clips.CreateClip;
using AshryverBot.Twitch.Helix.Models.Clips.GetClips;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class ClipsApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly ClipsApi _api;
    public ClipsApiTests() => _api = new ClipsApi(_client);

    [Fact]
    public async Task CreateClipAsync_uses_POST_clips_with_broadcaster_id()
    {
        _client.PostAsync<CreateClipResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new CreateClipResponse([]));

        await _api.CreateClipAsync("tok", "bc");

        await _client.Received(1).PostAsync<CreateClipResponse>(
            "clips", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.HasNoKey("has_delay")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateClipAsync_with_has_delay()
    {
        _client.PostAsync<CreateClipResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new CreateClipResponse([]));

        await _api.CreateClipAsync("tok", "bc", hasDelay: true);

        await _client.Received(1).PostAsync<CreateClipResponse>(
            "clips", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("has_delay", "true")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetClipsAsync_no_params_sends_empty_query()
    {
        _client.GetAsync<GetClipsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetClipsResponse([], null));

        await _api.GetClipsAsync("tok");

        await _client.Received(1).GetAsync<GetClipsResponse>(
            "clips", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.TotalCount() == 0),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetClipsAsync_all_filters()
    {
        _client.GetAsync<GetClipsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetClipsResponse([], null));

        var start = DateTimeOffset.UtcNow.AddDays(-1);
        var end = DateTimeOffset.UtcNow;
        await _api.GetClipsAsync("tok", "bc", "g", ["c1", "c2"],
            start, end, 20, "before-c", "after-c", isFeatured: false);

        await _client.Received(1).GetAsync<GetClipsResponse>(
            "clips", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("game_id", "g")
                && q.CountKey("id") == 2
                && q.CountKey("started_at") == 1 && q.CountKey("ended_at") == 1
                && q.Has("first", "20") && q.Has("before", "before-c") && q.Has("after", "after-c")
                && q.Has("is_featured", "false")),
            Arg.Any<CancellationToken>());
    }
}
