using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Channels.GetChannelInformation;

public record ChannelInformation
{
    [JsonPropertyName("broadcaster_id")]
    public string BroadcasterId { get; init; } = string.Empty;

    [JsonPropertyName("broadcaster_login")]
    public string BroadcasterLogin { get; init; } = string.Empty;

    [JsonPropertyName("broadcaster_name")]
    public string BroadcasterName { get; init; } = string.Empty;

    [JsonPropertyName("broadcaster_language")]
    public string BroadcasterLanguage { get; init; } = string.Empty;

    [JsonPropertyName("game_id")]
    public string GameId { get; init; } = string.Empty;

    [JsonPropertyName("game_name")]
    public string GameName { get; init; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; init; } = string.Empty;

    [JsonPropertyName("delay")]
    public int Delay { get; init; }

    [JsonPropertyName("tags")]
    public IReadOnlyCollection<string> Tags { get; init; } = [];

    [JsonPropertyName("content_classification_labels")]
    public IReadOnlyCollection<string> ContentClassificationLabels { get; init; } = [];

    [JsonPropertyName("is_branded_content")]
    public bool IsBrandedContent { get; init; }
}
