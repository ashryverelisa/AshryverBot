using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Chat.GetSharedChatSession;

public record GetSharedChatSessionResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<SharedChatSession> Data
);
