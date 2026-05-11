using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Whispers.SendWhisper;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class WhispersApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly WhispersApi _api;
    public WhispersApiTests() => _api = new WhispersApi(_client);

    [Fact]
    public async Task SendWhisperAsync_posts_to_whispers_with_query_and_body()
    {
        var body = new SendWhisperRequest("hi");
        await _api.SendWhisperAsync("tok", "from", "to", body);

        await _client.Received(1).PostAsync(
            "whispers", "tok", body,
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("from_user_id", "from") && q.Has("to_user_id", "to")
                && q.TotalCount() == 2),
            Arg.Any<CancellationToken>());
    }
}
