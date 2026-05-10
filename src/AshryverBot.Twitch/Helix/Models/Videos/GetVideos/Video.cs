using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Videos.GetVideos;

public record Video(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("stream_id")] string? StreamId,
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("user_login")] string UserLogin,
    [property: JsonPropertyName("user_name")] string UserName,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("created_at")] DateTimeOffset CreatedAt,
    [property: JsonPropertyName("published_at")] DateTimeOffset PublishedAt,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("thumbnail_url")] string ThumbnailUrl,
    [property: JsonPropertyName("viewable")] string Viewable,
    [property: JsonPropertyName("view_count")] long ViewCount,
    [property: JsonPropertyName("language")] string Language,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("duration")] string Duration,
    [property: JsonPropertyName("muted_segments")] IReadOnlyCollection<VideoMutedSegment>? MutedSegments
);

public record VideoMutedSegment(
    [property: JsonPropertyName("duration")] int Duration,
    [property: JsonPropertyName("offset")] int Offset
);
