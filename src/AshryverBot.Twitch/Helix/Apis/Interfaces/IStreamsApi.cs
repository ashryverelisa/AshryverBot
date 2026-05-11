using AshryverBot.Twitch.Helix.Models.Streams.CreateStreamMarker;
using AshryverBot.Twitch.Helix.Models.Streams.GetFollowedStreams;
using AshryverBot.Twitch.Helix.Models.Streams.GetStreamKey;
using AshryverBot.Twitch.Helix.Models.Streams.GetStreamMarkers;
using AshryverBot.Twitch.Helix.Models.Streams.GetStreams;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IStreamsApi
{
    Task<GetStreamKeyResponse> GetStreamKeyAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default);

    Task<GetStreamsResponse> GetStreamsAsync(
        string accessToken,
        IEnumerable<string>? userIds = null,
        IEnumerable<string>? userLogins = null,
        IEnumerable<string>? gameIds = null,
        string? type = null,
        IEnumerable<string>? languages = null,
        int? first = null,
        string? before = null,
        string? after = null,
        CancellationToken cancellationToken = default);

    Task<GetFollowedStreamsResponse> GetFollowedStreamsAsync(
        string accessToken,
        string userId,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default);

    Task<CreateStreamMarkerResponse> CreateStreamMarkerAsync(
        string accessToken,
        CreateStreamMarkerRequest body,
        CancellationToken cancellationToken = default);

    Task<GetStreamMarkersResponse> GetStreamMarkersAsync(
        string accessToken,
        string? userId = null,
        string? videoId = null,
        string? after = null,
        string? before = null,
        int? first = null,
        CancellationToken cancellationToken = default);
}
