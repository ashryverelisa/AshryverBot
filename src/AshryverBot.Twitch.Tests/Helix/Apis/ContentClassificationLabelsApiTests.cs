using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.ContentClassificationLabels.GetContentClassificationLabels;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class ContentClassificationLabelsApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly ContentClassificationLabelsApi _api;
    public ContentClassificationLabelsApiTests() => _api = new ContentClassificationLabelsApi(_client);

    [Fact]
    public async Task GetContentClassificationLabelsAsync_no_locale_sends_empty_query()
    {
        _client.GetAsync<GetContentClassificationLabelsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetContentClassificationLabelsResponse([]));

        await _api.GetContentClassificationLabelsAsync("tok");

        await _client.Received(1).GetAsync<GetContentClassificationLabelsResponse>(
            "content_classification_labels", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.TotalCount() == 0),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetContentClassificationLabelsAsync_with_locale()
    {
        _client.GetAsync<GetContentClassificationLabelsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetContentClassificationLabelsResponse([]));

        await _api.GetContentClassificationLabelsAsync("tok", "en-US");

        await _client.Received(1).GetAsync<GetContentClassificationLabelsResponse>(
            "content_classification_labels", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.Has("locale", "en-US")),
            Arg.Any<CancellationToken>());
    }
}
