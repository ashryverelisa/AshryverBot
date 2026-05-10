using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Chat.GetUserChatColor;

public record GetUserChatColorResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<UserChatColor> Data
);
