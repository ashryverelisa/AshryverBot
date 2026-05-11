using AshryverBot.Twitch.Helix.Models.ChannelPoints.CreateCustomReward;
using AshryverBot.Twitch.Helix.Models.ChannelPoints.GetCustomReward;
using AshryverBot.Twitch.Helix.Models.ChannelPoints.GetCustomRewardRedemption;
using AshryverBot.Twitch.Helix.Models.ChannelPoints.UpdateCustomReward;
using AshryverBot.Twitch.Helix.Models.ChannelPoints.UpdateRedemptionStatus;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IChannelPointsApi
{
    Task<CreateCustomRewardResponse> CreateCustomRewardAsync(
        string accessToken,
        string broadcasterId,
        CreateCustomRewardRequest body,
        CancellationToken cancellationToken = default);

    Task DeleteCustomRewardAsync(
        string accessToken,
        string broadcasterId,
        string id,
        CancellationToken cancellationToken = default);

    Task<GetCustomRewardResponse> GetCustomRewardAsync(
        string accessToken,
        string broadcasterId,
        IEnumerable<string>? ids = null,
        bool? onlyManageableRewards = null,
        CancellationToken cancellationToken = default);

    Task<GetCustomRewardRedemptionResponse> GetCustomRewardRedemptionAsync(
        string accessToken,
        string broadcasterId,
        string rewardId,
        string? status = null,
        IEnumerable<string>? ids = null,
        string? sort = null,
        string? after = null,
        int? first = null,
        CancellationToken cancellationToken = default);

    Task<UpdateCustomRewardResponse> UpdateCustomRewardAsync(
        string accessToken,
        string broadcasterId,
        string id,
        UpdateCustomRewardRequest body,
        CancellationToken cancellationToken = default);

    Task<UpdateRedemptionStatusResponse> UpdateRedemptionStatusAsync(
        string accessToken,
        string broadcasterId,
        string rewardId,
        IEnumerable<string> ids,
        UpdateRedemptionStatusRequest body,
        CancellationToken cancellationToken = default);
}
