using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Clips.CreateClip;

public record CreateClipResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ClipCreation> Data
);
