using AshryverBot.Infrastructure.Twitch.Tokens;
using AshryverBot.Twitch.Configuration;
using AshryverBot.Twitch.EventSub.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AshryverBot.Infrastructure.EventSub;

internal class BotEventSubAccessTokenProvider(
    IServiceScopeFactory scopeFactory,
    IOptions<TwitchOptions> twitchOptions) : IEventSubAccessTokenProvider
{
    private readonly TwitchOptions _twitchOptions = twitchOptions.Value;

    public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
        var botUserId = _twitchOptions.BotUserId
            ?? throw new InvalidOperationException("Twitch:BotUserId is not configured.");

        await using var scope = scopeFactory.CreateAsyncScope();
        var refresher = scope.ServiceProvider.GetRequiredService<ITwitchTokenRefresher>();

        var token = await refresher.GetValidAsync(botUserId, cancellationToken)
            ?? throw new InvalidOperationException("Bot token unavailable for EventSub subscription.");

        return token.AccessToken;
    }
}
