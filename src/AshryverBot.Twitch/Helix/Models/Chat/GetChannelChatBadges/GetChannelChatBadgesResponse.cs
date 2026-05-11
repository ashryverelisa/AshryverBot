using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Chat.GetChannelChatBadges;

public record GetChannelChatBadgesResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ChatBadgeSet> Data
);
