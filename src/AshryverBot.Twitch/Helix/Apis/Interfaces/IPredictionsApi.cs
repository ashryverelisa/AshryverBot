using AshryverBot.Twitch.Helix.Models.Predictions.CreatePrediction;
using AshryverBot.Twitch.Helix.Models.Predictions.EndPrediction;
using AshryverBot.Twitch.Helix.Models.Predictions.GetPredictions;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IPredictionsApi
{
    Task<GetPredictionsResponse> GetPredictionsAsync(
        string accessToken,
        string broadcasterId,
        IEnumerable<string>? ids = null,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default);

    Task<CreatePredictionResponse> CreatePredictionAsync(
        string accessToken,
        CreatePredictionRequest body,
        CancellationToken cancellationToken = default);

    Task<EndPredictionResponse> EndPredictionAsync(
        string accessToken,
        EndPredictionRequest body,
        CancellationToken cancellationToken = default);
}
