using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Common;

public record Pagination([property: JsonPropertyName("data")] string Cursor);