using AshryverBot.Twitch.Helix.Models.Clips.CreateClip;
using AshryverBot.Twitch.Helix.Models.Clips.GetClips;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IClipsApi
{
    Task<CreateClipResponse> CreateClipAsync(
        string accessToken,
        string broadcasterId,
        bool? hasDelay = null,
        CancellationToken cancellationToken = default);

    Task<GetClipsResponse> GetClipsAsync(
        string accessToken,
        string? broadcasterId = null,
        string? gameId = null,
        IEnumerable<string>? ids = null,
        DateTimeOffset? startedAt = null,
        DateTimeOffset? endedAt = null,
        int? first = null,
        string? before = null,
        string? after = null,
        bool? isFeatured = null,
        CancellationToken cancellationToken = default);
}
