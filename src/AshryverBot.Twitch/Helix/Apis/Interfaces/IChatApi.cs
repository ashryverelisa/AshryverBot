using AshryverBot.Twitch.Helix.Models.Chat.GetChannelChatBadges;
using AshryverBot.Twitch.Helix.Models.Chat.GetChannelEmotes;
using AshryverBot.Twitch.Helix.Models.Chat.GetChatSettings;
using AshryverBot.Twitch.Helix.Models.Chat.GetChatters;
using AshryverBot.Twitch.Helix.Models.Chat.GetEmoteSets;
using AshryverBot.Twitch.Helix.Models.Chat.GetGlobalChatBadges;
using AshryverBot.Twitch.Helix.Models.Chat.GetGlobalEmotes;
using AshryverBot.Twitch.Helix.Models.Chat.GetSharedChatSession;
using AshryverBot.Twitch.Helix.Models.Chat.GetUserChatColor;
using AshryverBot.Twitch.Helix.Models.Chat.GetUserEmotes;
using AshryverBot.Twitch.Helix.Models.Chat.SendChatAnnouncement;
using AshryverBot.Twitch.Helix.Models.Chat.SendChatMessage;
using AshryverBot.Twitch.Helix.Models.Chat.UpdateChatSettings;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IChatApi
{
    Task<GetChannelChatBadgesResponse> GetChannelChatBadgesAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default);

    Task<GetGlobalChatBadgesResponse> GetGlobalChatBadgesAsync(
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<GetChannelEmotesResponse> GetChannelEmotesAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default);

    Task<GetGlobalEmotesResponse> GetGlobalEmotesAsync(
        string accessToken,
        CancellationToken cancellationToken = default);

    Task<GetEmoteSetsResponse> GetEmoteSetsAsync(
        string accessToken,
        IEnumerable<string> emoteSetIds,
        CancellationToken cancellationToken = default);

    Task<GetUserEmotesResponse> GetUserEmotesAsync(
        string accessToken,
        string userId,
        string? broadcasterId = null,
        string? after = null,
        CancellationToken cancellationToken = default);

    Task<GetChatSettingsResponse> GetChatSettingsAsync(
        string accessToken,
        string broadcasterId,
        string? moderatorId = null,
        CancellationToken cancellationToken = default);

    Task<UpdateChatSettingsResponse> UpdateChatSettingsAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        UpdateChatSettingsRequest body,
        CancellationToken cancellationToken = default);

    Task SendChatAnnouncementAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        SendChatAnnouncementRequest body,
        CancellationToken cancellationToken = default);

    Task SendShoutoutAsync(
        string accessToken,
        string fromBroadcasterId,
        string toBroadcasterId,
        string moderatorId,
        CancellationToken cancellationToken = default);

    Task<SendChatMessageResponse> SendChatMessageAsync(
        string accessToken,
        SendChatMessageRequest body,
        CancellationToken cancellationToken = default);

    Task<GetUserChatColorResponse> GetUserChatColorAsync(
        string accessToken,
        IEnumerable<string> userIds,
        CancellationToken cancellationToken = default);

    Task UpdateUserChatColorAsync(
        string accessToken,
        string userId,
        string color,
        CancellationToken cancellationToken = default);

    Task<GetChattersResponse> GetChattersAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default);

    Task<GetSharedChatSessionResponse> GetSharedChatSessionAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default);
}
