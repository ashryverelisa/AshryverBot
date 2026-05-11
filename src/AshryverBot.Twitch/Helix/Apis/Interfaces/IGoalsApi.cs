using AshryverBot.Twitch.Helix.Models.Goals.GetCreatorGoals;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IGoalsApi
{
    Task<GetCreatorGoalsResponse> GetCreatorGoalsAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default);
}
