using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Chat.GetChannelChatBadges;

namespace AshryverBot.Twitch.Helix.Models.Chat.GetGlobalChatBadges;

public record GetGlobalChatBadgesResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ChatBadgeSet> Data
);
