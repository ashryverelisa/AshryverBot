using AshryverBot.Twitch.Helix.Models.Channels.GetAdSchedule;
using AshryverBot.Twitch.Helix.Models.Channels.GetChannelEditors;
using AshryverBot.Twitch.Helix.Models.Channels.GetChannelFollowers;
using AshryverBot.Twitch.Helix.Models.Channels.GetChannelInformation;
using AshryverBot.Twitch.Helix.Models.Channels.GetFollowedChannels;
using AshryverBot.Twitch.Helix.Models.Channels.GetVIPs;
using AshryverBot.Twitch.Helix.Models.Channels.ModifyChannelInformation;
using AshryverBot.Twitch.Helix.Models.Channels.Snooze;
using AshryverBot.Twitch.Helix.Models.Channels.StartCommercial;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IChannelsApi
{
    Task<GetChannelInformationResponse> GetChannelInformationAsync(
        string accessToken,
        IEnumerable<string> broadcasterIds,
        CancellationToken cancellationToken = default);

    Task ModifyChannelInformationAsync(
        string accessToken,
        string broadcasterId,
        ModifyChannelInformationRequest body,
        CancellationToken cancellationToken = default);

    Task<GetChannelEditorsResponse> GetChannelEditorsAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default);

    Task<GetFollowedChannelsResponse> GetFollowedChannelsAsync(
        string accessToken,
        string userId,
        string? broadcasterId = null,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default);

    Task<GetChannelFollowersResponse> GetChannelFollowersAsync(
        string accessToken,
        string broadcasterId,
        string? userId = null,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default);

    Task<StartCommercialResponse> StartCommercialAsync(
        string accessToken,
        StartCommercialRequest body,
        CancellationToken cancellationToken = default);

    Task<GetAdScheduleResponse> GetAdScheduleAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default);

    Task<SnoozeResponse> SnoozeNextAdAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default);

    Task<GetVIPsResponse> GetVIPsAsync(
        string accessToken,
        string broadcasterId,
        IEnumerable<string>? userIds = null,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default);

    Task AddChannelVipAsync(
        string accessToken,
        string userId,
        string broadcasterId,
        CancellationToken cancellationToken = default);

    Task RemoveChannelVipAsync(
        string accessToken,
        string userId,
        string broadcasterId,
        CancellationToken cancellationToken = default);
}
