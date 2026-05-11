using AshryverBot.Twitch.Helix.Models.Extensions.CreateExtensionSecret;
using AshryverBot.Twitch.Helix.Models.Extensions.GetExtensionBitsProducts;
using AshryverBot.Twitch.Helix.Models.Extensions.GetExtensionConfigurationSegment;
using AshryverBot.Twitch.Helix.Models.Extensions.GetExtensionLiveChannels;
using AshryverBot.Twitch.Helix.Models.Extensions.GetExtensions;
using AshryverBot.Twitch.Helix.Models.Extensions.GetExtensionSecrets;
using AshryverBot.Twitch.Helix.Models.Extensions.GetReleasedExtensions;
using AshryverBot.Twitch.Helix.Models.Extensions.SendExtensionChatMessage;
using AshryverBot.Twitch.Helix.Models.Extensions.SendExtensionPubSubMessage;
using AshryverBot.Twitch.Helix.Models.Extensions.SetExtensionConfigurationSegment;
using AshryverBot.Twitch.Helix.Models.Extensions.SetExtensionRequiredConfiguration;
using AshryverBot.Twitch.Helix.Models.Extensions.UpdateExtensionBitsProduct;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IExtensionsApi
{
    Task<GetExtensionConfigurationSegmentResponse> GetExtensionConfigurationSegmentAsync(
        string accessToken,
        string extensionId,
        string segment,
        string? broadcasterId = null,
        CancellationToken cancellationToken = default);

    Task SetExtensionConfigurationSegmentAsync(
        string accessToken,
        SetExtensionConfigurationSegmentRequest body,
        CancellationToken cancellationToken = default);

    Task SetExtensionRequiredConfigurationAsync(
        string accessToken,
        string broadcasterId,
        SetExtensionRequiredConfigurationRequest body,
        CancellationToken cancellationToken = default);

    Task SendExtensionPubSubMessageAsync(
        string accessToken,
        SendExtensionPubSubMessageRequest body,
        CancellationToken cancellationToken = default);

    Task<GetExtensionLiveChannelsResponse> GetExtensionLiveChannelsAsync(
        string accessToken,
        string extensionId,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default);

    Task<GetExtensionSecretsResponse> GetExtensionSecretsAsync(
        string accessToken,
        string extensionId,
        CancellationToken cancellationToken = default);

    Task<CreateExtensionSecretResponse> CreateExtensionSecretAsync(
        string accessToken,
        string extensionId,
        int? delay = null,
        CancellationToken cancellationToken = default);

    Task SendExtensionChatMessageAsync(
        string accessToken,
        string broadcasterId,
        SendExtensionChatMessageRequest body,
        CancellationToken cancellationToken = default);

    Task<GetExtensionsResponse> GetExtensionsAsync(
        string accessToken,
        string extensionId,
        string? extensionVersion = null,
        CancellationToken cancellationToken = default);

    Task<GetReleasedExtensionsResponse> GetReleasedExtensionsAsync(
        string accessToken,
        string extensionId,
        string? extensionVersion = null,
        CancellationToken cancellationToken = default);

    Task<GetExtensionBitsProductsResponse> GetExtensionBitsProductsAsync(
        string accessToken,
        bool? shouldIncludeAll = null,
        CancellationToken cancellationToken = default);

    Task<UpdateExtensionBitsProductResponse> UpdateExtensionBitsProductAsync(
        string accessToken,
        UpdateExtensionBitsProductRequest body,
        CancellationToken cancellationToken = default);
}
