using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.Search.SearchCategories;
using AshryverBot.Twitch.Helix.Models.Search.SearchChannels;

namespace AshryverBot.Twitch.Helix.Apis;

public class SearchApi(ITwitchClient client) : ISearchApi
{
    public Task<SearchCategoriesResponse> SearchCategoriesAsync(
        string accessToken,
        string query,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new List<KeyValuePair<string, string>>
        {
            new("query", query),
        };
        queryParams.AddIfNotNull("first", first);
        queryParams.AddIfNotNull("after", after);
        return client.GetAsync<SearchCategoriesResponse>("search/categories", accessToken, queryParams, cancellationToken);
    }

    public Task<SearchChannelsResponse> SearchChannelsAsync(
        string accessToken,
        string query,
        bool? liveOnly = null,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new List<KeyValuePair<string, string>>
        {
            new("query", query),
        };
        queryParams.AddIfNotNull("live_only", liveOnly);
        queryParams.AddIfNotNull("first", first);
        queryParams.AddIfNotNull("after", after);
        return client.GetAsync<SearchChannelsResponse>("search/channels", accessToken, queryParams, cancellationToken);
    }
}
