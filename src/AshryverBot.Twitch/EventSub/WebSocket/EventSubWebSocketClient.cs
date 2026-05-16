using System.Buffers;
using System.Net.WebSockets;
using System.Text.Json;
using AshryverBot.Twitch.EventSub.WebSocket.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Models.EventSub.Common;
using AshryverBot.Twitch.Helix.Models.EventSub.CreateEventSubSubscription;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AshryverBot.Twitch.EventSub.WebSocket;

public class EventSubWebSocketClient(
    IServiceScopeFactory scopeFactory,
    IEventSubAccessTokenProvider accessTokenProvider,
    IEnumerable<IEventSubConnectionObserver> connectionObservers,
    IOptions<EventSubWebSocketOptions> options,
    ILogger<EventSubWebSocketClient> logger)
{
    private const string TransportMethod = "websocket";

    private readonly EventSubWebSocketOptions _options = options.Value;
    private readonly IReadOnlyList<IEventSubConnectionObserver> _connectionObservers = connectionObservers.ToArray();

    public Task RunAsync(EventSubSubscriptionContext context, CancellationToken cancellationToken)
        => RunConnectionAsync(_options.Url, context, cancellationToken);

    private async Task RunConnectionAsync(string url, EventSubSubscriptionContext context, CancellationToken cancellationToken)
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

            switch (typeElement.GetString())
            {
                case EventSubMessageTypes.SessionWelcome:
                    if (subscribed) break;
                    var sessionId = root.GetProperty("payload").GetProperty("session").GetProperty("id").GetString()
                        ?? throw new InvalidOperationException("EventSub session_welcome missing session.id.");
                    await SubscribeAllAsync(sessionId, context, cancellationToken);
                    subscribed = true;
                    await NotifyConnectedAsync(cancellationToken);
                    break;

                case EventSubMessageTypes.SessionKeepalive:
                    break;

                case EventSubMessageTypes.SessionReconnect:
                    reconnectUrl = root.GetProperty("payload").GetProperty("session").GetProperty("reconnect_url").GetString();
                    logger.LogInformation("EventSub asked us to reconnect to {Url}.", reconnectUrl);
                    break;

                case EventSubMessageTypes.Notification:
                    await DispatchNotificationAsync(root, cancellationToken);
                    break;

                case EventSubMessageTypes.Revocation:
                    logger.LogWarning("EventSub subscription was revoked: {Payload}.", root.GetProperty("payload").ToString());
                    break;

                default:
                    logger.LogDebug("Ignoring EventSub message type {Type}.", typeElement.GetString());
                    break;
            }
        }

        if (!string.IsNullOrWhiteSpace(reconnectUrl) && !cancellationToken.IsCancellationRequested)
        {
            await RunConnectionAsync(reconnectUrl, context, cancellationToken);
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

    private async Task NotifyConnectedAsync(CancellationToken cancellationToken)
    {
        foreach (var observer in _connectionObservers)
        {
            try
            {
                await observer.OnConnectedAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "EventSub connection observer {Observer} failed in OnConnected.", observer.GetType().Name);
            }
        }
    }

    private async Task SubscribeAllAsync(string sessionId, EventSubSubscriptionContext context, CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var handlers = scope.ServiceProvider.GetServices<IEventSubHandler>().ToList();

        if (handlers.Count == 0)
        {
            logger.LogWarning("EventSub session welcomed but no handlers registered — nothing to subscribe to.");
            return;
        }

        var accessToken = await accessTokenProvider.GetAccessTokenAsync(cancellationToken);
        var eventSubApi = scope.ServiceProvider.GetRequiredService<IEventSubApi>();

        foreach (var handler in handlers)
        {
            var request = new CreateEventSubSubscriptionRequest(
                Type: handler.SubscriptionType,
                Version: handler.SubscriptionVersion,
                Condition: handler.BuildCondition(context),
                Transport: new EventSubTransport(
                    Method: TransportMethod,
                    Callback: null,
                    Secret: null,
                    SessionId: sessionId,
                    ConduitId: null,
                    ConnectedAt: null,
                    DisconnectedAt: null));

            try
            {
                await eventSubApi.CreateEventSubSubscriptionAsync(accessToken, request, cancellationToken);
                logger.LogInformation(
                    "Subscribed to EventSub {Type} v{Version}.",
                    handler.SubscriptionType,
                    handler.SubscriptionVersion);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Failed to subscribe to EventSub {Type} v{Version}.",
                    handler.SubscriptionType,
                    handler.SubscriptionVersion);
            }
        }
    }

    private async Task DispatchNotificationAsync(JsonElement root, CancellationToken cancellationToken)
    {
        var metadata = root.GetProperty("metadata");
        var subscriptionType = metadata.TryGetProperty("subscription_type", out var stElement)
            ? stElement.GetString() ?? string.Empty
            : string.Empty;
        var subscriptionVersion = metadata.TryGetProperty("subscription_version", out var svElement)
            ? svElement.GetString() ?? string.Empty
            : string.Empty;

        await using var scope = scopeFactory.CreateAsyncScope();
        var handler = scope.ServiceProvider.GetServices<IEventSubHandler>().FirstOrDefault(h =>
            string.Equals(h.SubscriptionType, subscriptionType, StringComparison.Ordinal) &&
            string.Equals(h.SubscriptionVersion, subscriptionVersion, StringComparison.Ordinal));

        if (handler is null)
        {
            logger.LogDebug("No handler for EventSub notification {Type} v{Version}.", subscriptionType, subscriptionVersion);
            return;
        }

        var payload = root.GetProperty("payload");
        var subscriptionId = payload.TryGetProperty("subscription", out var subElement) &&
            subElement.TryGetProperty("id", out var idElement)
                ? idElement.GetString() ?? string.Empty
                : string.Empty;

        var eventElement = payload.GetProperty("event").Clone();
        var notification = new EventSubNotification(subscriptionType, subscriptionVersion, subscriptionId, eventElement);

        try
        {
            await handler.HandleAsync(notification, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "EventSub handler for {Type} v{Version} failed.", subscriptionType, subscriptionVersion);
        }
    }
}
