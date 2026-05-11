using AshryverBot.Twitch.Helix.Models.Bits.GetBitsLeaderboard;
using AshryverBot.Twitch.Helix.Models.Bits.GetCheermotes;
using AshryverBot.Twitch.Helix.Models.Bits.GetExtensionTransactions;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IBitsApi
{
    Task<GetBitsLeaderboardResponse> GetBitsLeaderboardAsync(
        string accessToken,
        int? count = null,
        string? period = null,
        DateTimeOffset? startedAt = null,
        string? userId = null,
        CancellationToken cancellationToken = default);

    Task<GetCheermotesResponse> GetCheermotesAsync(
        string accessToken,
        string? broadcasterId = null,
        CancellationToken cancellationToken = default);

    Task<GetExtensionTransactionsResponse> GetExtensionTransactionsAsync(
        string accessToken,
        string extensionId,
        IEnumerable<string>? ids = null,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default);
}
