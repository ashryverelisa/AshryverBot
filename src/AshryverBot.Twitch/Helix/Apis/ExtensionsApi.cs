using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
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

namespace AshryverBot.Twitch.Helix.Apis;

public class ExtensionsApi(ITwitchClient client) : IExtensionsApi
{
    public Task<GetExtensionConfigurationSegmentResponse> GetExtensionConfigurationSegmentAsync(
        string accessToken,
        string extensionId,
        string segment,
        string? broadcasterId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("extension_id", extensionId),
            new("segment", segment),
        };
        query.AddIfNotNull("broadcaster_id", broadcasterId);
        return client.GetAsync<GetExtensionConfigurationSegmentResponse>(
            "extensions/configurations", accessToken, query, cancellationToken);
    }

    public Task SetExtensionConfigurationSegmentAsync(
        string accessToken,
        SetExtensionConfigurationSegmentRequest body,
        CancellationToken cancellationToken = default)
        => client.PutAsync("extensions/configurations", accessToken, body, queryParameters: null, cancellationToken);

    public Task SetExtensionRequiredConfigurationAsync(
        string accessToken,
        string broadcasterId,
        SetExtensionRequiredConfigurationRequest body,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        return client.PutAsync("extensions/required_configuration", accessToken, body, query, cancellationToken);
    }

    public Task SendExtensionPubSubMessageAsync(
        string accessToken,
        SendExtensionPubSubMessageRequest body,
        CancellationToken cancellationToken = default)
        => client.PostAsync("extensions/pubsub", accessToken, body, queryParameters: null, cancellationToken);

    public Task<GetExtensionLiveChannelsResponse> GetExtensionLiveChannelsAsync(
        string accessToken,
        string extensionId,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("extension_id", extensionId),
        };
        query.AddIfNotNull("first", first);
        query.AddIfNotNull("after", after);
        return client.GetAsync<GetExtensionLiveChannelsResponse>("extensions/live", accessToken, query, cancellationToken);
    }

    public Task<GetExtensionSecretsResponse> GetExtensionSecretsAsync(
        string accessToken,
        string extensionId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("extension_id", extensionId),
        };
        return client.GetAsync<GetExtensionSecretsResponse>("extensions/jwt/secrets", accessToken, query, cancellationToken);
    }

    public Task<CreateExtensionSecretResponse> CreateExtensionSecretAsync(
        string accessToken,
        string extensionId,
        int? delay = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("extension_id", extensionId),
        };
        query.AddIfNotNull("delay", delay);
        return client.PostAsync<CreateExtensionSecretResponse>("extensions/jwt/secrets", accessToken, query, cancellationToken);
    }

    public Task SendExtensionChatMessageAsync(
        string accessToken,
        string broadcasterId,
        SendExtensionChatMessageRequest body,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        return client.PostAsync("extensions/chat", accessToken, body, query, cancellationToken);
    }

    public Task<GetExtensionsResponse> GetExtensionsAsync(
        string accessToken,
        string extensionId,
        string? extensionVersion = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("extension_id", extensionId),
        };
        query.AddIfNotNull("extension_version", extensionVersion);
        return client.GetAsync<GetExtensionsResponse>("extensions", accessToken, query, cancellationToken);
    }

    public Task<GetReleasedExtensionsResponse> GetReleasedExtensionsAsync(
        string accessToken,
        string extensionId,
        string? extensionVersion = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("extension_id", extensionId),
        };
        query.AddIfNotNull("extension_version", extensionVersion);
        return client.GetAsync<GetReleasedExtensionsResponse>("extensions/released", accessToken, query, cancellationToken);
    }

    public Task<GetExtensionBitsProductsResponse> GetExtensionBitsProductsAsync(
        string accessToken,
        bool? shouldIncludeAll = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddIfNotNull("should_include_all", shouldIncludeAll);
        return client.GetAsync<GetExtensionBitsProductsResponse>("bits/extensions", accessToken, query, cancellationToken);
    }

    public Task<UpdateExtensionBitsProductResponse> UpdateExtensionBitsProductAsync(
        string accessToken,
        UpdateExtensionBitsProductRequest body,
        CancellationToken cancellationToken = default)
        => client.PutAsync<UpdateExtensionBitsProductRequest, UpdateExtensionBitsProductResponse>(
            "bits/extensions", accessToken, body, queryParameters: null, cancellationToken);
}
