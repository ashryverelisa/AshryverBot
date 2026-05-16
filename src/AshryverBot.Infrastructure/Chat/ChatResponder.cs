using AshryverBot.Infrastructure.Chat.Interfaces;
using AshryverBot.Infrastructure.Twitch.Tokens;
using AshryverBot.Twitch.Configuration;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Models.Chat.SendChatMessage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AshryverBot.Infrastructure.Chat;

public class ChatResponder(
    IChatApi chatApi,
    ITwitchTokenRefresher tokenRefresher,
    IOptions<TwitchOptions> twitchOptions,
    ILogger<ChatResponder> logger) : IChatResponder
{
    private readonly TwitchOptions _twitchOptions = twitchOptions.Value;

    public async Task SendAsync(
        string broadcasterId,
        string message,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(broadcasterId);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        var botUserId = _twitchOptions.BotUserId;
        if (string.IsNullOrWhiteSpace(botUserId))
        {
            logger.LogWarning("Cannot send chat message — Twitch:BotUserId is not configured.");
            return;
        }

        var token = await tokenRefresher.GetValidAsync(botUserId, cancellationToken);
        if (token is null)
        {
            logger.LogWarning("Cannot send chat message — bot token unavailable.");
            return;
        }

        await chatApi.SendChatMessageAsync(
            token.AccessToken,
            new SendChatMessageRequest
            {
                BroadcasterId = broadcasterId,
                SenderId = botUserId,
                Message = message,
            },
            cancellationToken);
    }
}
