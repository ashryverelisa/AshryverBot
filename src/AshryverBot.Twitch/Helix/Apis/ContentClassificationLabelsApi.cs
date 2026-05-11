using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.ContentClassificationLabels.GetContentClassificationLabels;

namespace AshryverBot.Twitch.Helix.Apis;

public class ContentClassificationLabelsApi(ITwitchClient client) : IContentClassificationLabelsApi
{
    public Task<GetContentClassificationLabelsResponse> GetContentClassificationLabelsAsync(
        string accessToken,
        string? locale = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddIfNotNull("locale", locale);
        return client.GetAsync<GetContentClassificationLabelsResponse>("content_classification_labels", accessToken, query, cancellationToken);
    }
}
