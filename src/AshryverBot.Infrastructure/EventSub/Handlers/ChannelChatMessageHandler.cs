using System.Text.Json;
using AshryverBot.Infrastructure.Chat;
using AshryverBot.Twitch.Configuration;
using AshryverBot.Twitch.EventSub.WebSocket;
using Microsoft.Extensions.Options;

namespace AshryverBot.Infrastructure.EventSub.Handlers;

internal class ChannelChatMessageHandler(
    IChatCommandDispatcher dispatcher,
    IOptions<TwitchOptions> twitchOptions) : IEventSubHandler
{
    private readonly TwitchOptions _twitchOptions = twitchOptions.Value;

    public string SubscriptionType => "channel.chat.message";

    public string SubscriptionVersion => "1";

    public JsonElement BuildCondition(EventSubSubscriptionContext context)
        => JsonSerializer.SerializeToElement(new
        {
            broadcaster_user_id = context.BroadcasterUserId,
            user_id = context.BotUserId,
        });

    public Task HandleAsync(EventSubNotification notification, CancellationToken cancellationToken)
    {
        var evt = notification.Event;
        var chatterUserId = evt.GetProperty("chatter_user_id").GetString() ?? string.Empty;

        if (string.Equals(chatterUserId, _twitchOptions.BotUserId, StringComparison.Ordinal))
            return Task.CompletedTask;

        var broadcasterUserId = evt.GetProperty("broadcaster_user_id").GetString() ?? string.Empty;
        var chatterUserLogin = evt.GetProperty("chatter_user_login").GetString() ?? string.Empty;
        var chatterUserName = evt.GetProperty("chatter_user_name").GetString() ?? chatterUserLogin;
        var messageId = evt.GetProperty("message_id").GetString() ?? string.Empty;
        var text = evt.GetProperty("message").GetProperty("text").GetString() ?? string.Empty;

        var message = new ChatMessage(
            broadcasterUserId,
            chatterUserId,
            chatterUserLogin,
            chatterUserName,
            messageId,
            text);

        return dispatcher.DispatchAsync(message, cancellationToken);
    }
}
