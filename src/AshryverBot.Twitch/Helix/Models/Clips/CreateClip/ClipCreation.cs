using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Clips.CreateClip;

public record ClipCreation(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("edit_url")] string EditUrl
);
