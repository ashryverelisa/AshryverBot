using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Charity.GetCharityCampaign;
using AshryverBot.Twitch.Helix.Models.Charity.GetCharityCampaignDonations;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class CharityApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly CharityApi _api;
    public CharityApiTests() => _api = new CharityApi(_client);

    [Fact]
    public async Task GetCharityCampaignAsync_calls_endpoint_with_broadcaster_id()
    {
        _client.GetAsync<GetCharityCampaignResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetCharityCampaignResponse([]));

        await _api.GetCharityCampaignAsync("tok", "bc");

        await _client.Received(1).GetAsync<GetCharityCampaignResponse>(
            "charity/campaigns", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetCharityCampaignDonationsAsync_required_only()
    {
        _client.GetAsync<GetCharityCampaignDonationsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetCharityCampaignDonationsResponse([], null));

        await _api.GetCharityCampaignDonationsAsync("tok", "bc");

        await _client.Received(1).GetAsync<GetCharityCampaignDonationsResponse>(
            "charity/donations", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.HasNoKey("first") && q.HasNoKey("after")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetCharityCampaignDonationsAsync_with_paging()
    {
        _client.GetAsync<GetCharityCampaignDonationsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetCharityCampaignDonationsResponse([], null));

        await _api.GetCharityCampaignDonationsAsync("tok", "bc", 20, "cur");

        await _client.Received(1).GetAsync<GetCharityCampaignDonationsResponse>(
            "charity/donations", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("first", "20") && q.Has("after", "cur")),
            Arg.Any<CancellationToken>());
    }
}
