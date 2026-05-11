using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
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
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class ExtensionsApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly ExtensionsApi _api;
    public ExtensionsApiTests() => _api = new ExtensionsApi(_client);

    [Fact]
    public async Task GetExtensionConfigurationSegmentAsync_required_only()
    {
        _client.GetAsync<GetExtensionConfigurationSegmentResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetExtensionConfigurationSegmentResponse([]));

        await _api.GetExtensionConfigurationSegmentAsync("tok", "ext", "global");

        await _client.Received(1).GetAsync<GetExtensionConfigurationSegmentResponse>(
            "extensions/configurations", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("extension_id", "ext") && q.Has("segment", "global")
                && q.HasNoKey("broadcaster_id")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetExtensionConfigurationSegmentAsync_with_broadcaster_id()
    {
        _client.GetAsync<GetExtensionConfigurationSegmentResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetExtensionConfigurationSegmentResponse([]));

        await _api.GetExtensionConfigurationSegmentAsync("tok", "ext", "broadcaster", "bc");

        await _client.Received(1).GetAsync<GetExtensionConfigurationSegmentResponse>(
            "extensions/configurations", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SetExtensionConfigurationSegmentAsync_uses_PUT_no_query()
    {
        var body = new SetExtensionConfigurationSegmentRequest("ext", "global", null, "{}", "1.0");
        await _api.SetExtensionConfigurationSegmentAsync("tok", body);

        await _client.Received(1).PutAsync(
            "extensions/configurations", "tok", body, null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SetExtensionRequiredConfigurationAsync_uses_PUT_with_broadcaster_id()
    {
        var body = new SetExtensionRequiredConfigurationRequest("ext", "1.0", "cfg");
        await _api.SetExtensionRequiredConfigurationAsync("tok", "bc", body);

        await _client.Received(1).PutAsync(
            "extensions/required_configuration", "tok", body,
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SendExtensionPubSubMessageAsync_uses_POST_no_query()
    {
        var body = new SendExtensionPubSubMessageRequest(["broadcast"], "bc", true, "{}");
        await _api.SendExtensionPubSubMessageAsync("tok", body);

        await _client.Received(1).PostAsync(
            "extensions/pubsub", "tok", body, null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetExtensionLiveChannelsAsync_required_only()
    {
        _client.GetAsync<GetExtensionLiveChannelsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetExtensionLiveChannelsResponse([], null));

        await _api.GetExtensionLiveChannelsAsync("tok", "ext");

        await _client.Received(1).GetAsync<GetExtensionLiveChannelsResponse>(
            "extensions/live", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("extension_id", "ext") && q.HasNoKey("first") && q.HasNoKey("after")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetExtensionLiveChannelsAsync_with_paging()
    {
        _client.GetAsync<GetExtensionLiveChannelsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetExtensionLiveChannelsResponse([], null));

        await _api.GetExtensionLiveChannelsAsync("tok", "ext", 25, "cur");

        await _client.Received(1).GetAsync<GetExtensionLiveChannelsResponse>(
            "extensions/live", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("first", "25") && q.Has("after", "cur")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetExtensionSecretsAsync_calls_jwt_secrets()
    {
        _client.GetAsync<GetExtensionSecretsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetExtensionSecretsResponse([]));

        await _api.GetExtensionSecretsAsync("tok", "ext");

        await _client.Received(1).GetAsync<GetExtensionSecretsResponse>(
            "extensions/jwt/secrets", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("extension_id", "ext") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateExtensionSecretAsync_no_delay()
    {
        _client.PostAsync<CreateExtensionSecretResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new CreateExtensionSecretResponse([]));

        await _api.CreateExtensionSecretAsync("tok", "ext");

        await _client.Received(1).PostAsync<CreateExtensionSecretResponse>(
            "extensions/jwt/secrets", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("extension_id", "ext") && q.HasNoKey("delay")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateExtensionSecretAsync_with_delay()
    {
        _client.PostAsync<CreateExtensionSecretResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new CreateExtensionSecretResponse([]));

        await _api.CreateExtensionSecretAsync("tok", "ext", 30);

        await _client.Received(1).PostAsync<CreateExtensionSecretResponse>(
            "extensions/jwt/secrets", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("delay", "30")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SendExtensionChatMessageAsync_uses_POST_chat_with_broadcaster_id()
    {
        var body = new SendExtensionChatMessageRequest("hi", "ext", "1.0");
        await _api.SendExtensionChatMessageAsync("tok", "bc", body);

        await _client.Received(1).PostAsync(
            "extensions/chat", "tok", body,
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.Has("broadcaster_id", "bc")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetExtensionsAsync_with_version()
    {
        _client.GetAsync<GetExtensionsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetExtensionsResponse([]));

        await _api.GetExtensionsAsync("tok", "ext", "1.0");

        await _client.Received(1).GetAsync<GetExtensionsResponse>(
            "extensions", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("extension_id", "ext") && q.Has("extension_version", "1.0")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetExtensionsAsync_without_version()
    {
        _client.GetAsync<GetExtensionsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetExtensionsResponse([]));

        await _api.GetExtensionsAsync("tok", "ext");

        await _client.Received(1).GetAsync<GetExtensionsResponse>(
            "extensions", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("extension_id", "ext") && q.HasNoKey("extension_version")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetReleasedExtensionsAsync_uses_released_endpoint()
    {
        _client.GetAsync<GetReleasedExtensionsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetReleasedExtensionsResponse([]));

        await _api.GetReleasedExtensionsAsync("tok", "ext", "1.0");

        await _client.Received(1).GetAsync<GetReleasedExtensionsResponse>(
            "extensions/released", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("extension_id", "ext") && q.Has("extension_version", "1.0")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetExtensionBitsProductsAsync_no_flag()
    {
        _client.GetAsync<GetExtensionBitsProductsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetExtensionBitsProductsResponse([]));

        await _api.GetExtensionBitsProductsAsync("tok");

        await _client.Received(1).GetAsync<GetExtensionBitsProductsResponse>(
            "bits/extensions", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.HasNoKey("should_include_all")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetExtensionBitsProductsAsync_with_flag()
    {
        _client.GetAsync<GetExtensionBitsProductsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetExtensionBitsProductsResponse([]));

        await _api.GetExtensionBitsProductsAsync("tok", shouldIncludeAll: true);

        await _client.Received(1).GetAsync<GetExtensionBitsProductsResponse>(
            "bits/extensions", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.Has("should_include_all", "true")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateExtensionBitsProductAsync_uses_PUT_with_body()
    {
        _client.PutAsync<UpdateExtensionBitsProductRequest, UpdateExtensionBitsProductResponse>(
            null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new UpdateExtensionBitsProductResponse([]));

        var body = new UpdateExtensionBitsProductRequest("sku",
            new ExtensionBitsProductCost(100, "bits"), "Display", null, null, null);
        await _api.UpdateExtensionBitsProductAsync("tok", body);

        await _client.Received(1).PutAsync<UpdateExtensionBitsProductRequest, UpdateExtensionBitsProductResponse>(
            "bits/extensions", "tok", body, null, Arg.Any<CancellationToken>());
    }
}
