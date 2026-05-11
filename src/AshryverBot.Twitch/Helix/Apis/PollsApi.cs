using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.Polls.CreatePoll;
using AshryverBot.Twitch.Helix.Models.Polls.EndPoll;
using AshryverBot.Twitch.Helix.Models.Polls.GetPolls;

namespace AshryverBot.Twitch.Helix.Apis;

public class PollsApi(ITwitchClient client) : IPollsApi
{
    public Task<GetPollsResponse> GetPollsAsync(
        string accessToken,
        string broadcasterId,
        IEnumerable<string>? ids = null,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        query.AddMany("id", ids);
        query.AddIfNotNull("first", first);
        query.AddIfNotNull("after", after);
        return client.GetAsync<GetPollsResponse>("polls", accessToken, query, cancellationToken);
    }

    public Task<CreatePollResponse> CreatePollAsync(
        string accessToken,
        CreatePollRequest body,
        CancellationToken cancellationToken = default)
        => client.PostAsync<CreatePollRequest, CreatePollResponse>("polls", accessToken, body, queryParameters: null, cancellationToken);

    public Task<EndPollResponse> EndPollAsync(
        string accessToken,
        EndPollRequest body,
        CancellationToken cancellationToken = default)
        => client.PatchAsync<EndPollRequest, EndPollResponse>("polls", accessToken, body, queryParameters: null, cancellationToken);
}
