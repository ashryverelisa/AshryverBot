using AshryverBot.Twitch.Helix.Models.Search.SearchCategories;
using AshryverBot.Twitch.Helix.Models.Search.SearchChannels;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface ISearchApi
{
    Task<SearchCategoriesResponse> SearchCategoriesAsync(
        string accessToken,
        string query,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default);

    Task<SearchChannelsResponse> SearchChannelsAsync(
        string accessToken,
        string query,
        bool? liveOnly = null,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default);
}
