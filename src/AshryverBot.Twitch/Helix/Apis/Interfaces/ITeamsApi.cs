using AshryverBot.Twitch.Helix.Models.Teams.GetChannelTeams;
using AshryverBot.Twitch.Helix.Models.Teams.GetTeams;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface ITeamsApi
{
    Task<GetChannelTeamsResponse> GetChannelTeamsAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default);

    Task<GetTeamsResponse> GetTeamsAsync(
        string accessToken,
        string? name = null,
        string? id = null,
        CancellationToken cancellationToken = default);
}
