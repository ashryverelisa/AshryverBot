using System.Text.Json;

namespace AshryverBot.Twitch.EventSub.WebSocket;

public record EventSubNotification(
    string SubscriptionType,
    string SubscriptionVersion,
    string SubscriptionId,
    JsonElement Event);
