using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Streams.CreateStreamMarker;

namespace AshryverBot.Twitch.Helix.Models.Streams.GetStreamMarkers;

public record StreamMarkerUserGroup(
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("user_login")] string UserLogin,
    [property: JsonPropertyName("user_name")] string UserName,
    [property: JsonPropertyName("videos")] IReadOnlyCollection<StreamMarkerVideo> Videos
);

public record StreamMarkerVideo(
    [property: JsonPropertyName("video_id")] string VideoId,
    [property: JsonPropertyName("markers")] IReadOnlyCollection<StreamMarker> Markers
);
