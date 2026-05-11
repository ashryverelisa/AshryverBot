using AshryverBot.Twitch.Helix.Models.Games.GetGames;
using AshryverBot.Twitch.Helix.Models.Games.GetTopGames;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IGamesApi
{
    Task<GetTopGamesResponse> GetTopGamesAsync(
        string accessToken,
        int? first = null,
        string? after = null,
        string? before = null,
        CancellationToken cancellationToken = default);

    Task<GetGamesResponse> GetGamesAsync(
        string accessToken,
        IEnumerable<string>? ids = null,
        IEnumerable<string>? names = null,
        IEnumerable<string>? igdbIds = null,
        CancellationToken cancellationToken = default);
}
