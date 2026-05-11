using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Polls.CreatePoll;
using AshryverBot.Twitch.Helix.Models.Polls.EndPoll;
using AshryverBot.Twitch.Helix.Models.Polls.GetPolls;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class PollsApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly PollsApi _api;
    public PollsApiTests() => _api = new PollsApi(_client);

    [Fact]
    public async Task GetPollsAsync_required_only()
    {
        _client.GetAsync<GetPollsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetPollsResponse([], null));

        await _api.GetPollsAsync("tok", "bc");

        await _client.Received(1).GetAsync<GetPollsResponse>(
            "polls", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetPollsAsync_with_ids_and_paging()
    {
        _client.GetAsync<GetPollsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetPollsResponse([], null));

        await _api.GetPollsAsync("tok", "bc", ["p1", "p2"], 10, "cur");

        await _client.Received(1).GetAsync<GetPollsResponse>(
            "polls", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.CountKey("id") == 2
                && q.Has("first", "10") && q.Has("after", "cur")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreatePollAsync_forwards_body_via_POST_no_query()
    {
        _client.PostAsync<CreatePollRequest, CreatePollResponse>(null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new CreatePollResponse([]));

        var body = new CreatePollRequest { BroadcasterId = "bc", Title = "T", Duration = 60 };
        await _api.CreatePollAsync("tok", body);

        await _client.Received(1).PostAsync<CreatePollRequest, CreatePollResponse>(
            "polls", "tok", body, null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task EndPollAsync_forwards_body_via_PATCH_no_query()
    {
        _client.PatchAsync<EndPollRequest, EndPollResponse>(null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new EndPollResponse([]));

        var body = new EndPollRequest("bc", "p", "TERMINATED");
        await _api.EndPollAsync("tok", body);

        await _client.Received(1).PatchAsync<EndPollRequest, EndPollResponse>(
            "polls", "tok", body, null, Arg.Any<CancellationToken>());
    }
}
