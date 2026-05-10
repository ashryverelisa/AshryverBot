using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Chat.GetSharedChatSession;

public record SharedChatSession(
    [property: JsonPropertyName("session_id")] string SessionId,
    [property: JsonPropertyName("host_broadcaster_id")] string HostBroadcasterId,
    [property: JsonPropertyName("participants")] IReadOnlyCollection<SharedChatParticipant> Participants,
    [property: JsonPropertyName("created_at")] DateTimeOffset CreatedAt,
    [property: JsonPropertyName("updated_at")] DateTimeOffset UpdatedAt
);

public record SharedChatParticipant(
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId
);
