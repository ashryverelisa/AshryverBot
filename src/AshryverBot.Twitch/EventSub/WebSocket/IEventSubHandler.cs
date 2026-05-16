using System.Text.Json;

namespace AshryverBot.Twitch.EventSub.WebSocket;

public interface IEventSubHandler
{
    string SubscriptionType { get; }

    string SubscriptionVersion { get; }

    JsonElement BuildCondition(EventSubSubscriptionContext context);

    Task HandleAsync(EventSubNotification notification, CancellationToken cancellationToken);
}
