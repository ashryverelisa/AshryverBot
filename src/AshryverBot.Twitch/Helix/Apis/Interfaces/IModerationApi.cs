using AshryverBot.Twitch.Helix.Models.Moderation.AddBlockedTerm;
using AshryverBot.Twitch.Helix.Models.Moderation.BanUser;
using AshryverBot.Twitch.Helix.Models.Moderation.CheckAutoModStatus;
using AshryverBot.Twitch.Helix.Models.Moderation.GetAutoModSettings;
using AshryverBot.Twitch.Helix.Models.Moderation.GetBannedUsers;
using AshryverBot.Twitch.Helix.Models.Moderation.GetBlockedTerms;
using AshryverBot.Twitch.Helix.Models.Moderation.GetModeratedChannels;
using AshryverBot.Twitch.Helix.Models.Moderation.GetModerators;
using AshryverBot.Twitch.Helix.Models.Moderation.GetShieldModeStatus;
using AshryverBot.Twitch.Helix.Models.Moderation.GetUnbanRequests;
using AshryverBot.Twitch.Helix.Models.Moderation.ManageHeldAutoModMessages;
using AshryverBot.Twitch.Helix.Models.Moderation.ResolveUnbanRequest;
using AshryverBot.Twitch.Helix.Models.Moderation.UpdateAutoModSettings;
using AshryverBot.Twitch.Helix.Models.Moderation.UpdateShieldModeStatus;
using AshryverBot.Twitch.Helix.Models.Moderation.WarnChatUser;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IModerationApi
{
    Task<CheckAutoModStatusResponse> CheckAutoModStatusAsync(
        string accessToken,
        string broadcasterId,
        CheckAutoModStatusRequest body,
        CancellationToken cancellationToken = default);

    Task ManageHeldAutoModMessagesAsync(
        string accessToken,
        ManageHeldAutoModMessagesRequest body,
        CancellationToken cancellationToken = default);

    Task<GetAutoModSettingsResponse> GetAutoModSettingsAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        CancellationToken cancellationToken = default);

    Task<UpdateAutoModSettingsResponse> UpdateAutoModSettingsAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        UpdateAutoModSettingsRequest body,
        CancellationToken cancellationToken = default);

    Task<GetBannedUsersResponse> GetBannedUsersAsync(
        string accessToken,
        string broadcasterId,
        IEnumerable<string>? userIds = null,
        int? first = null,
        string? after = null,
        string? before = null,
        CancellationToken cancellationToken = default);

    Task<BanUserResponse> BanUserAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        BanUserRequest body,
        CancellationToken cancellationToken = default);

    Task UnbanUserAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string userId,
        CancellationToken cancellationToken = default);

    Task<GetUnbanRequestsResponse> GetUnbanRequestsAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string status,
        string? userId = null,
        string? after = null,
        int? first = null,
        CancellationToken cancellationToken = default);

    Task<ResolveUnbanRequestResponse> ResolveUnbanRequestAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string unbanRequestId,
        string status,
        string? resolutionText = null,
        CancellationToken cancellationToken = default);

    Task<GetBlockedTermsResponse> GetBlockedTermsAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string? after = null,
        int? first = null,
        CancellationToken cancellationToken = default);

    Task<AddBlockedTermResponse> AddBlockedTermAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        AddBlockedTermRequest body,
        CancellationToken cancellationToken = default);

    Task RemoveBlockedTermAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string id,
        CancellationToken cancellationToken = default);

    Task DeleteChatMessagesAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string? messageId = null,
        CancellationToken cancellationToken = default);

    Task<GetModeratedChannelsResponse> GetModeratedChannelsAsync(
        string accessToken,
        string userId,
        string? after = null,
        int? first = null,
        CancellationToken cancellationToken = default);

    Task<GetModeratorsResponse> GetModeratorsAsync(
        string accessToken,
        string broadcasterId,
        IEnumerable<string>? userIds = null,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default);

    Task AddChannelModeratorAsync(
        string accessToken,
        string broadcasterId,
        string userId,
        CancellationToken cancellationToken = default);

    Task RemoveChannelModeratorAsync(
        string accessToken,
        string broadcasterId,
        string userId,
        CancellationToken cancellationToken = default);

    Task<GetShieldModeStatusResponse> GetShieldModeStatusAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        CancellationToken cancellationToken = default);

    Task<UpdateShieldModeStatusResponse> UpdateShieldModeStatusAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        UpdateShieldModeStatusRequest body,
        CancellationToken cancellationToken = default);

    Task<WarnChatUserResponse> WarnChatUserAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        WarnChatUserRequest body,
        CancellationToken cancellationToken = default);
}
