using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Models.Goals.GetCreatorGoals;

namespace AshryverBot.Twitch.Helix.Apis;

public class GoalsApi(ITwitchClient client) : IGoalsApi
{
    public Task<GetCreatorGoalsResponse> GetCreatorGoalsAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        return client.GetAsync<GetCreatorGoalsResponse>("goals", accessToken, query, cancellationToken);
    }
}
