using AshryverBot.Twitch.Helix.Models.HypeTrain.GetHypeTrainEvents;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IHypeTrainApi
{
    Task<GetHypeTrainEventsResponse> GetHypeTrainEventsAsync(
        string accessToken,
        string broadcasterId,
        int? first = null,
        string? id = null,
        string? after = null,
        CancellationToken cancellationToken = default);
}
