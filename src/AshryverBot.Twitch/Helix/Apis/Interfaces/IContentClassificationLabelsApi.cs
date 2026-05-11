using AshryverBot.Twitch.Helix.Models.ContentClassificationLabels.GetContentClassificationLabels;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IContentClassificationLabelsApi
{
    Task<GetContentClassificationLabelsResponse> GetContentClassificationLabelsAsync(
        string accessToken,
        string? locale = null,
        CancellationToken cancellationToken = default);
}
