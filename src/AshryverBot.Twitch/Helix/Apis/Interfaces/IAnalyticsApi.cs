using AshryverBot.Twitch.Helix.Models.Analytics.GetExtensionAnalytics;
using AshryverBot.Twitch.Helix.Models.Analytics.GetGameAnalytics;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IAnalyticsApi
{
    Task<GetExtensionAnalyticsResponse> GetExtensionAnalyticsAsync(
        string accessToken,
        string? extensionId = null,
        string? type = null,
        DateTimeOffset? startedAt = null,
        DateTimeOffset? endedAt = null,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default);

    Task<GetGameAnalyticsResponse> GetGameAnalyticsAsync(
        string accessToken,
        string? gameId = null,
        string? type = null,
        DateTimeOffset? startedAt = null,
        DateTimeOffset? endedAt = null,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default);
}
