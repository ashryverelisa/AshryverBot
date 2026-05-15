using System.Buffers;
using System.Net.WebSockets;
using System.Text.Json;
using AshryverBot.Infrastructure.Twitch.Tokens;
using AshryverBot.Twitch.Configuration;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Models.EventSub.Common;
using AshryverBot.Twitch.Helix.Models.EventSub.CreateEventSubSubscription;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AshryverBot.Infrastructure.Chat;

public class EventSubChatClient(
    IServiceScopeFactory scopeFactory,
    IOptions<TwitchOptions> twitchOptions,
    ILogger<EventSubChatClient> logger) : BackgroundService
{
    private const string DefaultUrl = "wss://eventsub.wss.twitch.tv/ws";

    private readonly TwitchOptions _twitchOptions = twitchOptions.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (string.IsNullOrWhiteSpace(_twitchOptions.BroadcasterId))
        {
            logger.LogWarning("Twitch:BroadcasterId is not configured — EventSub chat client disabled.");
            return;
        }

        if (string.IsNullOrWhiteSpace(_twitchOptions.BotUserId))
        {
            logger.LogWarning("Twitch:BotUserId is not configured — EventSub chat client disabled.");
            return;
        }

        var backoff = TimeSpan.FromSeconds(2);
        var maxBackoff = TimeSpan.FromMinutes(2);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RunConnectionAsync(DefaultUrl, stoppingToken);
                backoff = TimeSpan.FromSeconds(2);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "EventSub chat client errored; reconnecting in {Delay}.", backoff);
                try { await Task.Delay(backoff, stoppingToken); }
                catch (OperationCanceledException) { return; }
                backoff = TimeSpan.FromSeconds(Math.Min(backoff.TotalSeconds * 2, maxBackoff.TotalSeconds));
            }
        }
    }

    private async Task RunConnectionAsync(string url, CancellationToken cancellationToken)
    {
        using var socket = new ClientWebSocket();
        await socket.ConnectAsync(new Uri(url), cancellationToken);
        logger.LogInformation("Connected to EventSub WebSocket {Url}.", url);

        string? reconnectUrl = null;
        var subscribed = false;

        var buffer = new ArrayBufferWriter<byte>(8192);

        while (socket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
        {
            buffer.ResetWrittenCount();
            var frame = await ReceiveTextAsync(socket, buffer, cancellationToken);
            if (frame is null) break;

            using var document = JsonDocument.Parse(frame.Value);
            var root = document.RootElement;
            if (!root.TryGetProperty("metadata", out var metadata) ||
                !metadata.TryGetProperty("message_type", out var typeElement))
            {
                logger.LogWarning("Received EventSub frame without metadata.message_type.");
                continue;
            }

            var messageType = typeElement.GetString();
            switch (messageType)
            {
                case "session_welcome":
                    if (!subscribed)
                    {
                        var sessionId = root.GetProperty("payload").GetProperty("session").GetProperty("id").GetString();
                        if (string.IsNullOrWhiteSpace(sessionId))
                            throw new InvalidOperationException("EventSub session_welcome missing session.id.");
                        await SubscribeAsync(sessionId, cancellationToken);
                        subscribed = true;
                    }
                    break;

                case "session_keepalive":
                    break;

                case "session_reconnect":
                    reconnectUrl = root.GetProperty("payload").GetProperty("session").GetProperty("reconnect_url").GetString();
                    logger.LogInformation("EventSub asked us to reconnect to {Url}.", reconnectUrl);
                    break;

                case "notification":
                    await HandleNotificationAsync(root, cancellationToken);
                    break;

                case "revocation":
                    logger.LogWarning("EventSub subscription was revoked: {Payload}.", root.GetProperty("payload").ToString());
                    break;

                default:
                    logger.LogDebug("Ignoring EventSub message type {Type}.", messageType);
                    break;
            }
        }

        if (!string.IsNullOrWhiteSpace(reconnectUrl) && !cancellationToken.IsCancellationRequested)
        {
            await RunConnectionAsync(reconnectUrl, cancellationToken);
        }
    }

    private async Task<ReadOnlyMemory<byte>?> ReceiveTextAsync(
        ClientWebSocket socket,
        ArrayBufferWriter<byte> buffer,
        CancellationToken cancellationToken)
    {
        var rented = new byte[4096];
        while (true)
        {
            var result = await socket.ReceiveAsync(rented, cancellationToken);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                logger.LogInformation("EventSub socket closed: {Status} {Description}.",
                    result.CloseStatus, result.CloseStatusDescription);
                return null;
            }

            buffer.Write(rented.AsSpan(0, result.Count));
            if (result.EndOfMessage) return buffer.WrittenMemory;
        }
    }

    private async Task SubscribeAsync(string sessionId, CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var refresher = scope.ServiceProvider.GetRequiredService<ITwitchTokenRefresher>();
        var eventSub = scope.ServiceProvider.GetRequiredService<IEventSubApi>();

        var botUserId = _twitchOptions.BotUserId!;
        var broadcasterId = _twitchOptions.BroadcasterId!;

        var token = await refresher.GetValidAsync(botUserId, cancellationToken)
            ?? throw new InvalidOperationException("Bot token unavailable; cannot subscribe to EventSub chat messages.");

        var conditionJson = JsonSerializer.SerializeToElement(new
        {
            broadcaster_user_id = broadcasterId,
            user_id = botUserId,
        });

        var request = new CreateEventSubSubscriptionRequest(
            Type: "channel.chat.message",
            Version: "1",
            Condition: conditionJson,
            Transport: new EventSubTransport(
                Method: "websocket",
                Callback: null,
                Secret: null,
                SessionId: sessionId,
                ConduitId: null,
                ConnectedAt: null,
                DisconnectedAt: null));

        await eventSub.CreateEventSubSubscriptionAsync(token.AccessToken, request, cancellationToken);
        logger.LogInformation("Subscribed to channel.chat.message for broadcaster {BroadcasterId}.", broadcasterId);
    }

    private async Task HandleNotificationAsync(JsonElement root, CancellationToken cancellationToken)
    {
        var metadata = root.GetProperty("metadata");
        var subscriptionType = metadata.TryGetProperty("subscription_type", out var stElement)
            ? stElement.GetString()
            : null;

        if (!string.Equals(subscriptionType, "channel.chat.message", StringComparison.Ordinal))
        {
            logger.LogDebug("Ignoring notification of type {Type}.", subscriptionType);
            return;
        }

        var evt = root.GetProperty("payload").GetProperty("event");
        var broadcasterUserId = evt.GetProperty("broadcaster_user_id").GetString() ?? string.Empty;
        var chatterUserId = evt.GetProperty("chatter_user_id").GetString() ?? string.Empty;
        var chatterUserLogin = evt.GetProperty("chatter_user_login").GetString() ?? string.Empty;
        var chatterUserName = evt.GetProperty("chatter_user_name").GetString() ?? chatterUserLogin;
        var messageId = evt.GetProperty("message_id").GetString() ?? string.Empty;
        var text = evt.GetProperty("message").GetProperty("text").GetString() ?? string.Empty;

        // Ignore the bot's own messages.
        if (string.Equals(chatterUserId, _twitchOptions.BotUserId, StringComparison.Ordinal))
            return;

        var message = new ChatMessage(
            broadcasterUserId,
            chatterUserId,
            chatterUserLogin,
            chatterUserName,
            messageId,
            text);

        await using var scope = scopeFactory.CreateAsyncScope();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IChatCommandDispatcher>();
        await dispatcher.DispatchAsync(message, cancellationToken);
    }
}
