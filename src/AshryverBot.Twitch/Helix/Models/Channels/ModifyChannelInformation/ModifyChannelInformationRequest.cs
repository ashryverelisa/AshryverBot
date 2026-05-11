using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Channels.ModifyChannelInformation;

public record ModifyChannelInformationRequest
{
    [JsonPropertyName("game_id")]
    public string? GameId { get; init; }

    [JsonPropertyName("broadcaster_language")]
    public string? BroadcasterLanguage { get; init; }

    [JsonPropertyName("title")]
    public string? Title { get; init; }

    [JsonPropertyName("delay")]
    public int? Delay { get; init; }

    [JsonPropertyName("tags")]
    public IReadOnlyCollection<string>? Tags { get; init; }

    [JsonPropertyName("content_classification_labels")]
    public IReadOnlyCollection<ContentClassificationLabelUpdate>? ContentClassificationLabels { get; init; }

    [JsonPropertyName("is_branded_content")]
    public bool? IsBrandedContent { get; init; }
}

public record ContentClassificationLabelUpdate(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("is_enabled")] bool IsEnabled
);
