using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Chat.GetChannelChatBadges;

public record ChatBadgeSet(
    [property: JsonPropertyName("set_id")] string SetId,
    [property: JsonPropertyName("versions")] IReadOnlyCollection<ChatBadgeVersion> Versions
);

public record ChatBadgeVersion(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("image_url_1x")] string ImageUrl1x,
    [property: JsonPropertyName("image_url_2x")] string ImageUrl2x,
    [property: JsonPropertyName("image_url_4x")] string ImageUrl4x,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("click_action")] string? ClickAction,
    [property: JsonPropertyName("click_url")] string? ClickUrl
);
