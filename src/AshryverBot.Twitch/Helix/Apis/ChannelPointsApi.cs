using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.ChannelPoints.CreateCustomReward;
using AshryverBot.Twitch.Helix.Models.ChannelPoints.GetCustomReward;
using AshryverBot.Twitch.Helix.Models.ChannelPoints.GetCustomRewardRedemption;
using AshryverBot.Twitch.Helix.Models.ChannelPoints.UpdateCustomReward;
using AshryverBot.Twitch.Helix.Models.ChannelPoints.UpdateRedemptionStatus;

namespace AshryverBot.Twitch.Helix.Apis;

public class ChannelPointsApi(ITwitchClient client) : IChannelPointsApi
{
    public Task<CreateCustomRewardResponse> CreateCustomRewardAsync(
        string accessToken,
        string broadcasterId,
        CreateCustomRewardRequest body,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        return client.PostAsync<CreateCustomRewardRequest, CreateCustomRewardResponse>(
            "channel_points/custom_rewards", accessToken, body, query, cancellationToken);
    }

    public Task DeleteCustomRewardAsync(
        string accessToken,
        string broadcasterId,
        string id,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("id", id),
        };
        return client.DeleteAsync("channel_points/custom_rewards", accessToken, query, cancellationToken);
    }

    public Task<GetCustomRewardResponse> GetCustomRewardAsync(
        string accessToken,
        string broadcasterId,
        IEnumerable<string>? ids = null,
        bool? onlyManageableRewards = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        query.AddMany("id", ids);
        query.AddIfNotNull("only_manageable_rewards", onlyManageableRewards);
        return client.GetAsync<GetCustomRewardResponse>("channel_points/custom_rewards", accessToken, query, cancellationToken);
    }

    public Task<GetCustomRewardRedemptionResponse> GetCustomRewardRedemptionAsync(
        string accessToken,
        string broadcasterId,
        string rewardId,
        string? status = null,
        IEnumerable<string>? ids = null,
        string? sort = null,
        string? after = null,
        int? first = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("reward_id", rewardId),
        };
        query.AddIfNotNull("status", status);
        query.AddMany("id", ids);
        query.AddIfNotNull("sort", sort);
        query.AddIfNotNull("after", after);
        query.AddIfNotNull("first", first);
        return client.GetAsync<GetCustomRewardRedemptionResponse>(
            "channel_points/custom_rewards/redemptions", accessToken, query, cancellationToken);
    }

    public Task<UpdateCustomRewardResponse> UpdateCustomRewardAsync(
        string accessToken,
        string broadcasterId,
        string id,
        UpdateCustomRewardRequest body,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("id", id),
        };
        return client.PatchAsync<UpdateCustomRewardRequest, UpdateCustomRewardResponse>(
            "channel_points/custom_rewards", accessToken, body, query, cancellationToken);
    }

    public Task<UpdateRedemptionStatusResponse> UpdateRedemptionStatusAsync(
        string accessToken,
        string broadcasterId,
        string rewardId,
        IEnumerable<string> ids,
        UpdateRedemptionStatusRequest body,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("reward_id", rewardId),
        };
        query.AddMany("id", ids);
        return client.PatchAsync<UpdateRedemptionStatusRequest, UpdateRedemptionStatusResponse>(
            "channel_points/custom_rewards/redemptions", accessToken, body, query, cancellationToken);
    }
}
