using AshryverBot.Twitch.Helix.Models.Videos.DeleteVideos;
using AshryverBot.Twitch.Helix.Models.Videos.GetVideos;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IVideosApi
{
    Task<GetVideosResponse> GetVideosAsync(
        string accessToken,
        IEnumerable<string>? ids = null,
        string? userId = null,
        string? gameId = null,
        string? language = null,
        string? period = null,
        string? sort = null,
        string? type = null,
        int? first = null,
        string? after = null,
        string? before = null,
        CancellationToken cancellationToken = default);

    Task<DeleteVideosResponse> DeleteVideosAsync(
        string accessToken,
        IEnumerable<string> ids,
        CancellationToken cancellationToken = default);
}
