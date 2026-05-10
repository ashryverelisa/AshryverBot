using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Moderation.CheckAutoModStatus;

public record CheckAutoModStatusRequest(
    [property: JsonPropertyName("data")] IReadOnlyCollection<CheckAutoModStatusMessage> Data
);

public record CheckAutoModStatusMessage(
    [property: JsonPropertyName("msg_id")] string MsgId,
    [property: JsonPropertyName("msg_text")] string MsgText
);
