using AshryverBot.Twitch.Helix.Models.Polls.CreatePoll;
using AshryverBot.Twitch.Helix.Models.Polls.EndPoll;
using AshryverBot.Twitch.Helix.Models.Polls.GetPolls;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IPollsApi
{
    Task<GetPollsResponse> GetPollsAsync(
        string accessToken,
        string broadcasterId,
        IEnumerable<string>? ids = null,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default);

    Task<CreatePollResponse> CreatePollAsync(
        string accessToken,
        CreatePollRequest body,
        CancellationToken cancellationToken = default);

    Task<EndPollResponse> EndPollAsync(
        string accessToken,
        EndPollRequest body,
        CancellationToken cancellationToken = default);
}
