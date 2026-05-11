using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Clips.GetClips;

public record Clip(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("embed_url")] string EmbedUrl,
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("broadcaster_name")] string BroadcasterName,
    [property: JsonPropertyName("creator_id")] string CreatorId,
    [property: JsonPropertyName("creator_name")] string CreatorName,
    [property: JsonPropertyName("video_id")] string VideoId,
    [property: JsonPropertyName("game_id")] string GameId,
    [property: JsonPropertyName("language")] string Language,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("view_count")] long ViewCount,
    [property: JsonPropertyName("created_at")] DateTimeOffset CreatedAt,
    [property: JsonPropertyName("thumbnail_url")] string ThumbnailUrl,
    [property: JsonPropertyName("duration")] double Duration,
    [property: JsonPropertyName("vod_offset")] int? VodOffset,
    [property: JsonPropertyName("is_featured")] bool IsFeatured
);
