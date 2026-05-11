using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.Channels.GetAdSchedule;
using AshryverBot.Twitch.Helix.Models.Channels.GetChannelEditors;
using AshryverBot.Twitch.Helix.Models.Channels.GetChannelFollowers;
using AshryverBot.Twitch.Helix.Models.Channels.GetChannelInformation;
using AshryverBot.Twitch.Helix.Models.Channels.GetFollowedChannels;
using AshryverBot.Twitch.Helix.Models.Channels.GetVIPs;
using AshryverBot.Twitch.Helix.Models.Channels.ModifyChannelInformation;
using AshryverBot.Twitch.Helix.Models.Channels.Snooze;
using AshryverBot.Twitch.Helix.Models.Channels.StartCommercial;

namespace AshryverBot.Twitch.Helix.Apis;

public class ChannelsApi(ITwitchClient client) : IChannelsApi
{
    public Task<GetChannelInformationResponse> GetChannelInformationAsync(
        string accessToken,
        IEnumerable<string> broadcasterIds,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddMany("broadcaster_id", broadcasterIds);
        return client.GetAsync<GetChannelInformationResponse>("channels", accessToken, query, cancellationToken);
    }

    public Task ModifyChannelInformationAsync(
        string accessToken,
        string broadcasterId,
        ModifyChannelInformationRequest body,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        return client.PatchAsync("channels", accessToken, body, query, cancellationToken);
    }

    public Task<GetChannelEditorsResponse> GetChannelEditorsAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        return client.GetAsync<GetChannelEditorsResponse>("channels/editors", accessToken, query, cancellationToken);
    }

    public Task<GetFollowedChannelsResponse> GetFollowedChannelsAsync(
        string accessToken,
        string userId,
        string? broadcasterId = null,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("user_id", userId),
        };
        query.AddIfNotNull("broadcaster_id", broadcasterId);
        query.AddIfNotNull("first", first);
        query.AddIfNotNull("after", after);
        return client.GetAsync<GetFollowedChannelsResponse>("channels/followed", accessToken, query, cancellationToken);
    }

    public Task<GetChannelFollowersResponse> GetChannelFollowersAsync(
        string accessToken,
        string broadcasterId,
        string? userId = null,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        query.AddIfNotNull("user_id", userId);
        query.AddIfNotNull("first", first);
        query.AddIfNotNull("after", after);
        return client.GetAsync<GetChannelFollowersResponse>("channels/followers", accessToken, query, cancellationToken);
    }

    public Task<StartCommercialResponse> StartCommercialAsync(
        string accessToken,
        StartCommercialRequest body,
        CancellationToken cancellationToken = default)
        => client.PostAsync<StartCommercialRequest, StartCommercialResponse>("channels/commercial", accessToken, body, queryParameters: null, cancellationToken);

    public Task<GetAdScheduleResponse> GetAdScheduleAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        return client.GetAsync<GetAdScheduleResponse>("channels/ads", accessToken, query, cancellationToken);
    }

    public Task<SnoozeResponse> SnoozeNextAdAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        return client.PostAsync<SnoozeResponse>("channels/ads/schedule/snooze", accessToken, query, cancellationToken);
    }

    public Task<GetVIPsResponse> GetVIPsAsync(
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
        return client.GetAsync<GetVIPsResponse>("channels/vips", accessToken, query, cancellationToken);
    }

    public Task AddChannelVipAsync(
        string accessToken,
        string userId,
        string broadcasterId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("user_id", userId),
            new("broadcaster_id", broadcasterId),
        };
        return client.PostAsync("channels/vips", accessToken, query, cancellationToken);
    }

    public Task RemoveChannelVipAsync(
        string accessToken,
        string userId,
        string broadcasterId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("user_id", userId),
            new("broadcaster_id", broadcasterId),
        };
        return client.DeleteAsync("channels/vips", accessToken, query, cancellationToken);
    }
}
