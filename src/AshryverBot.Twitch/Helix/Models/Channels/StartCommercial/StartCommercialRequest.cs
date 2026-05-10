using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Channels.StartCommercial;

public record StartCommercialRequest
{
    [JsonPropertyName("broadcaster_id")]
    public string BroadcasterId { get; set; } = string.Empty;

    [JsonPropertyName("length")]
    public int Length { get; set; }
}