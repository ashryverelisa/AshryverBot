using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Predictions.CreatePrediction;
using AshryverBot.Twitch.Helix.Models.Predictions.EndPrediction;
using AshryverBot.Twitch.Helix.Models.Predictions.GetPredictions;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class PredictionsApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly PredictionsApi _api;
    public PredictionsApiTests() => _api = new PredictionsApi(_client);

    [Fact]
    public async Task GetPredictionsAsync_required_only()
    {
        _client.GetAsync<GetPredictionsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetPredictionsResponse([], null));

        await _api.GetPredictionsAsync("tok", "bc");

        await _client.Received(1).GetAsync<GetPredictionsResponse>(
            "predictions", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetPredictionsAsync_with_ids_and_paging()
    {
        _client.GetAsync<GetPredictionsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetPredictionsResponse([], null));

        await _api.GetPredictionsAsync("tok", "bc", ["p1"], 5, "cur");

        await _client.Received(1).GetAsync<GetPredictionsResponse>(
            "predictions", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("id", "p1")
                && q.Has("first", "5") && q.Has("after", "cur")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreatePredictionAsync_uses_POST_predictions()
    {
        _client.PostAsync<CreatePredictionRequest, CreatePredictionResponse>(null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new CreatePredictionResponse([]));

        var body = new CreatePredictionRequest { BroadcasterId = "bc", Title = "T", PredictionWindow = 60 };
        await _api.CreatePredictionAsync("tok", body);

        await _client.Received(1).PostAsync<CreatePredictionRequest, CreatePredictionResponse>(
            "predictions", "tok", body, null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task EndPredictionAsync_uses_PATCH_predictions()
    {
        _client.PatchAsync<EndPredictionRequest, EndPredictionResponse>(null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new EndPredictionResponse([]));

        var body = new EndPredictionRequest("bc", "p", "RESOLVED", "outcome");
        await _api.EndPredictionAsync("tok", body);

        await _client.Received(1).PatchAsync<EndPredictionRequest, EndPredictionResponse>(
            "predictions", "tok", body, null, Arg.Any<CancellationToken>());
    }
}
