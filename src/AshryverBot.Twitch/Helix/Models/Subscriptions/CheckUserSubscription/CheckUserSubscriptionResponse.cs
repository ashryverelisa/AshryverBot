using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Subscriptions.CheckUserSubscription;

public record CheckUserSubscriptionResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<UserSubscription> Data
);
