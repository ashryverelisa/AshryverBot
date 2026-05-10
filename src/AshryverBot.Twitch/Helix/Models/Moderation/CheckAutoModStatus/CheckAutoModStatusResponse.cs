using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Moderation.CheckAutoModStatus;

public record CheckAutoModStatusResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<CheckAutoModStatusResult> Data
);

public record CheckAutoModStatusResult(
    [property: JsonPropertyName("msg_id")] string MsgId,
    [property: JsonPropertyName("is_permitted")] bool IsPermitted
);
