using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.HypeTrain.GetHypeTrainEvents;

public record GetHypeTrainEventsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<HypeTrainEvent> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
