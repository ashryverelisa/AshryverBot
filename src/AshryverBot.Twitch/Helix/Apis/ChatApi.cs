using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
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

namespace AshryverBot.Twitch.Helix.Apis;

public class ChatApi(ITwitchClient client) : IChatApi
{
    public Task<GetChannelChatBadgesResponse> GetChannelChatBadgesAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        return client.GetAsync<GetChannelChatBadgesResponse>("chat/badges", accessToken, query, cancellationToken);
    }

    public Task<GetGlobalChatBadgesResponse> GetGlobalChatBadgesAsync(
        string accessToken,
        CancellationToken cancellationToken = default)
        => client.GetAsync<GetGlobalChatBadgesResponse>("chat/badges/global", accessToken, queryParameters: null, cancellationToken);

    public Task<GetChannelEmotesResponse> GetChannelEmotesAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        return client.GetAsync<GetChannelEmotesResponse>("chat/emotes", accessToken, query, cancellationToken);
    }

    public Task<GetGlobalEmotesResponse> GetGlobalEmotesAsync(
        string accessToken,
        CancellationToken cancellationToken = default)
        => client.GetAsync<GetGlobalEmotesResponse>("chat/emotes/global", accessToken, queryParameters: null, cancellationToken);

    public Task<GetEmoteSetsResponse> GetEmoteSetsAsync(
        string accessToken,
        IEnumerable<string> emoteSetIds,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddMany("emote_set_id", emoteSetIds);
        return client.GetAsync<GetEmoteSetsResponse>("chat/emotes/set", accessToken, query, cancellationToken);
    }

    public Task<GetUserEmotesResponse> GetUserEmotesAsync(
        string accessToken,
        string userId,
        string? broadcasterId = null,
        string? after = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("user_id", userId),
        };
        query.AddIfNotNull("broadcaster_id", broadcasterId);
        query.AddIfNotNull("after", after);
        return client.GetAsync<GetUserEmotesResponse>("chat/emotes/user", accessToken, query, cancellationToken);
    }

    public Task<GetChatSettingsResponse> GetChatSettingsAsync(
        string accessToken,
        string broadcasterId,
        string? moderatorId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        query.AddIfNotNull("moderator_id", moderatorId);
        return client.GetAsync<GetChatSettingsResponse>("chat/settings", accessToken, query, cancellationToken);
    }

    public Task<UpdateChatSettingsResponse> UpdateChatSettingsAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        UpdateChatSettingsRequest body,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
        };
        return client.PatchAsync<UpdateChatSettingsRequest, UpdateChatSettingsResponse>(
            "chat/settings", accessToken, body, query, cancellationToken);
    }

    public Task SendChatAnnouncementAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        SendChatAnnouncementRequest body,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
        };
        return client.PostAsync("chat/announcements", accessToken, body, query, cancellationToken);
    }

    public Task SendShoutoutAsync(
        string accessToken,
        string fromBroadcasterId,
        string toBroadcasterId,
        string moderatorId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("from_broadcaster_id", fromBroadcasterId),
            new("to_broadcaster_id", toBroadcasterId),
            new("moderator_id", moderatorId),
        };
        return client.PostAsync("chat/shoutouts", accessToken, query, cancellationToken);
    }

    public Task<SendChatMessageResponse> SendChatMessageAsync(
        string accessToken,
        SendChatMessageRequest body,
        CancellationToken cancellationToken = default)
        => client.PostAsync<SendChatMessageRequest, SendChatMessageResponse>(
            "chat/messages", accessToken, body, queryParameters: null, cancellationToken);

    public Task<GetUserChatColorResponse> GetUserChatColorAsync(
        string accessToken,
        IEnumerable<string> userIds,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddMany("user_id", userIds);
        return client.GetAsync<GetUserChatColorResponse>("chat/color", accessToken, query, cancellationToken);
    }

    public Task UpdateUserChatColorAsync(
        string accessToken,
        string userId,
        string color,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("user_id", userId),
            new("color", color),
        };
        return client.PutAsync("chat/color", accessToken, query, cancellationToken);
    }

    public Task<GetChattersResponse> GetChattersAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
        };
        query.AddIfNotNull("first", first);
        query.AddIfNotNull("after", after);
        return client.GetAsync<GetChattersResponse>("chat/chatters", accessToken, query, cancellationToken);
    }

    public Task<GetSharedChatSessionResponse> GetSharedChatSessionAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        return client.GetAsync<GetSharedChatSessionResponse>("chat/shared_chat/session", accessToken, query, cancellationToken);
    }
}
