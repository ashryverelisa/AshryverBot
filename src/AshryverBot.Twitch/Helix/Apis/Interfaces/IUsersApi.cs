using AshryverBot.Twitch.Helix.Models.Users.GetUserActiveExtensions;
using AshryverBot.Twitch.Helix.Models.Users.GetUserBlockList;
using AshryverBot.Twitch.Helix.Models.Users.GetUserExtensions;
using AshryverBot.Twitch.Helix.Models.Users.GetUsers;
using AshryverBot.Twitch.Helix.Models.Users.UpdateUser;
using AshryverBot.Twitch.Helix.Models.Users.UpdateUserExtensions;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IUsersApi
{
    Task<GetUsersResponse> GetUsersAsync(
        string accessToken,
        IEnumerable<string>? ids = null,
        IEnumerable<string>? logins = null,
        CancellationToken cancellationToken = default);

    Task<UpdateUserResponse> UpdateUserAsync(
        string accessToken,
        string? description = null,
        CancellationToken cancellationToken = default);

    Task<GetUserBlockListResponse> GetUserBlockListAsync(
        string accessToken,
        string broadcasterId,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default);

    Task BlockUserAsync(
        string accessToken,
        string targetUserId,
        string? sourceContext = null,
        string? reason = null,
        CancellationToken cancellationToken = default);

    Task UnblockUserAsync(
        string accessToken,
        string targetUserId,
        CancellationToken cancellationToken = default);

    Task<GetUserExtensionsResponse> GetUserExtensionsAsync(
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<GetUserActiveExtensionsResponse> GetUserActiveExtensionsAsync(
        string accessToken,
        string? userId = null,
        CancellationToken cancellationToken = default);

    Task<UpdateUserExtensionsResponse> UpdateUserExtensionsAsync(
        string accessToken,
        UpdateUserExtensionsRequest body,
        CancellationToken cancellationToken = default);
}
