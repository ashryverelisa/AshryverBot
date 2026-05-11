using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
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

namespace AshryverBot.Twitch.Helix.Apis;

public class ModerationApi(ITwitchClient client) : IModerationApi
{
    public Task<CheckAutoModStatusResponse> CheckAutoModStatusAsync(
        string accessToken,
        string broadcasterId,
        CheckAutoModStatusRequest body,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        return client.PostAsync<CheckAutoModStatusRequest, CheckAutoModStatusResponse>(
            "moderation/enforcements/status", accessToken, body, query, cancellationToken);
    }

    public Task ManageHeldAutoModMessagesAsync(
        string accessToken,
        ManageHeldAutoModMessagesRequest body,
        CancellationToken cancellationToken = default)
        => client.PostAsync("moderation/automod/message", accessToken, body, queryParameters: null, cancellationToken);

    public Task<GetAutoModSettingsResponse> GetAutoModSettingsAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
        };
        return client.GetAsync<GetAutoModSettingsResponse>("moderation/automod/settings", accessToken, query, cancellationToken);
    }

    public Task<UpdateAutoModSettingsResponse> UpdateAutoModSettingsAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        UpdateAutoModSettingsRequest body,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
        };
        return client.PutAsync<UpdateAutoModSettingsRequest, UpdateAutoModSettingsResponse>(
            "moderation/automod/settings", accessToken, body, query, cancellationToken);
    }

    public Task<GetBannedUsersResponse> GetBannedUsersAsync(
        string accessToken,
        string broadcasterId,
        IEnumerable<string>? userIds = null,
        int? first = null,
        string? after = null,
        string? before = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        query.AddMany("user_id", userIds);
        query.AddIfNotNull("first", first);
        query.AddIfNotNull("after", after);
        query.AddIfNotNull("before", before);
        return client.GetAsync<GetBannedUsersResponse>("moderation/banned", accessToken, query, cancellationToken);
    }

    public Task<BanUserResponse> BanUserAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        BanUserRequest body,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
        };
        return client.PostAsync<BanUserRequest, BanUserResponse>(
            "moderation/bans", accessToken, body, query, cancellationToken);
    }

    public Task UnbanUserAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
            new("user_id", userId),
        };
        return client.DeleteAsync("moderation/bans", accessToken, query, cancellationToken);
    }

    public Task<GetUnbanRequestsResponse> GetUnbanRequestsAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string status,
        string? userId = null,
        string? after = null,
        int? first = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
            new("status", status),
        };
        query.AddIfNotNull("user_id", userId);
        query.AddIfNotNull("after", after);
        query.AddIfNotNull("first", first);
        return client.GetAsync<GetUnbanRequestsResponse>("moderation/unban_requests", accessToken, query, cancellationToken);
    }

    public Task<ResolveUnbanRequestResponse> ResolveUnbanRequestAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string unbanRequestId,
        string status,
        string? resolutionText = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
            new("unban_request_id", unbanRequestId),
            new("status", status),
        };
        query.AddIfNotNull("resolution_text", resolutionText);
        return client.PatchAsync<object, ResolveUnbanRequestResponse>(
            "moderation/unban_requests", accessToken, new object(), query, cancellationToken);
    }

    public Task<GetBlockedTermsResponse> GetBlockedTermsAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string? after = null,
        int? first = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
        };
        query.AddIfNotNull("after", after);
        query.AddIfNotNull("first", first);
        return client.GetAsync<GetBlockedTermsResponse>("moderation/blocked_terms", accessToken, query, cancellationToken);
    }

    public Task<AddBlockedTermResponse> AddBlockedTermAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        AddBlockedTermRequest body,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
        };
        return client.PostAsync<AddBlockedTermRequest, AddBlockedTermResponse>(
            "moderation/blocked_terms", accessToken, body, query, cancellationToken);
    }

    public Task RemoveBlockedTermAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string id,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
            new("id", id),
        };
        return client.DeleteAsync("moderation/blocked_terms", accessToken, query, cancellationToken);
    }

    public Task DeleteChatMessagesAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string? messageId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
        };
        query.AddIfNotNull("message_id", messageId);
        return client.DeleteAsync("moderation/chat", accessToken, query, cancellationToken);
    }

    public Task<GetModeratedChannelsResponse> GetModeratedChannelsAsync(
        string accessToken,
        string userId,
        string? after = null,
        int? first = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("user_id", userId),
        };
        query.AddIfNotNull("after", after);
        query.AddIfNotNull("first", first);
        return client.GetAsync<GetModeratedChannelsResponse>("moderation/channels", accessToken, query, cancellationToken);
    }

    public Task<GetModeratorsResponse> GetModeratorsAsync(
        string accessToken,
        string broadcasterId,
        IEnumerable<string>? userIds = null,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        query.AddMany("user_id", userIds);
        query.AddIfNotNull("first", first);
        query.AddIfNotNull("after", after);
        return client.GetAsync<GetModeratorsResponse>("moderation/moderators", accessToken, query, cancellationToken);
    }

    public Task AddChannelModeratorAsync(
        string accessToken,
        string broadcasterId,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("user_id", userId),
        };
        return client.PostAsync("moderation/moderators", accessToken, query, cancellationToken);
    }

    public Task RemoveChannelModeratorAsync(
        string accessToken,
        string broadcasterId,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("user_id", userId),
        };
        return client.DeleteAsync("moderation/moderators", accessToken, query, cancellationToken);
    }

    public Task<GetShieldModeStatusResponse> GetShieldModeStatusAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
        };
        return client.GetAsync<GetShieldModeStatusResponse>("moderation/shield_mode", accessToken, query, cancellationToken);
    }

    public Task<UpdateShieldModeStatusResponse> UpdateShieldModeStatusAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        UpdateShieldModeStatusRequest body,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
        };
        return client.PutAsync<UpdateShieldModeStatusRequest, UpdateShieldModeStatusResponse>(
            "moderation/shield_mode", accessToken, body, query, cancellationToken);
    }

    public Task<WarnChatUserResponse> WarnChatUserAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        WarnChatUserRequest body,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
        };
        return client.PostAsync<WarnChatUserRequest, WarnChatUserResponse>(
            "moderation/warnings", accessToken, body, query, cancellationToken);
    }
}
