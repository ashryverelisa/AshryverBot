using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.Predictions.CreatePrediction;
using AshryverBot.Twitch.Helix.Models.Predictions.EndPrediction;
using AshryverBot.Twitch.Helix.Models.Predictions.GetPredictions;

namespace AshryverBot.Twitch.Helix.Apis;

public class PredictionsApi(ITwitchClient client) : IPredictionsApi
{
    public Task<GetPredictionsResponse> GetPredictionsAsync(
        string accessToken,
        string broadcasterId,
        IEnumerable<string>? ids = null,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        query.AddMany("id", ids);
        query.AddIfNotNull("first", first);
        query.AddIfNotNull("after", after);
        return client.GetAsync<GetPredictionsResponse>("predictions", accessToken, query, cancellationToken);
    }

    public Task<CreatePredictionResponse> CreatePredictionAsync(
        string accessToken,
        CreatePredictionRequest body,
        CancellationToken cancellationToken = default)
        => client.PostAsync<CreatePredictionRequest, CreatePredictionResponse>("predictions", accessToken, body, queryParameters: null, cancellationToken);

    public Task<EndPredictionResponse> EndPredictionAsync(
        string accessToken,
        EndPredictionRequest body,
        CancellationToken cancellationToken = default)
        => client.PatchAsync<EndPredictionRequest, EndPredictionResponse>("predictions", accessToken, body, queryParameters: null, cancellationToken);
}
