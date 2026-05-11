using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Extensions.Common;

public record Extension(
    [property: JsonPropertyName("author_name")] string AuthorName,
    [property: JsonPropertyName("bits_enabled")] bool BitsEnabled,
    [property: JsonPropertyName("can_install")] bool CanInstall,
    [property: JsonPropertyName("configuration_location")] string ConfigurationLocation,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("eula_tos_url")] string EulaTosUrl,
    [property: JsonPropertyName("has_chat_support")] bool HasChatSupport,
    [property: JsonPropertyName("icon_url")] string IconUrl,
    [property: JsonPropertyName("icon_urls")] IReadOnlyDictionary<string, string> IconUrls,
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("privacy_policy_url")] string PrivacyPolicyUrl,
    [property: JsonPropertyName("request_identity_link")] bool RequestIdentityLink,
    [property: JsonPropertyName("screenshot_urls")] IReadOnlyCollection<string> ScreenshotUrls,
    [property: JsonPropertyName("state")] string State,
    [property: JsonPropertyName("subscriptions_support_level")] string SubscriptionsSupportLevel,
    [property: JsonPropertyName("summary")] string Summary,
    [property: JsonPropertyName("support_email")] string SupportEmail,
    [property: JsonPropertyName("version")] string Version,
    [property: JsonPropertyName("viewer_summary")] string ViewerSummary,
    [property: JsonPropertyName("views")] ExtensionViews Views,
    [property: JsonPropertyName("allowlisted_config_urls")] IReadOnlyCollection<string> AllowlistedConfigUrls,
    [property: JsonPropertyName("allowlisted_panel_urls")] IReadOnlyCollection<string> AllowlistedPanelUrls
);

public record ExtensionViews(
    [property: JsonPropertyName("mobile")] ExtensionMobileView? Mobile,
    [property: JsonPropertyName("panel")] ExtensionPanelView? Panel,
    [property: JsonPropertyName("video_overlay")] ExtensionVideoOverlayView? VideoOverlay,
    [property: JsonPropertyName("component")] ExtensionComponentView? Component,
    [property: JsonPropertyName("config")] ExtensionConfigView? Config
);

public record ExtensionMobileView(
    [property: JsonPropertyName("viewer_url")] string ViewerUrl
);

public record ExtensionPanelView(
    [property: JsonPropertyName("viewer_url")] string ViewerUrl,
    [property: JsonPropertyName("height")] int Height,
    [property: JsonPropertyName("can_link_external_content")] bool CanLinkExternalContent
);

public record ExtensionVideoOverlayView(
    [property: JsonPropertyName("viewer_url")] string ViewerUrl,
    [property: JsonPropertyName("can_link_external_content")] bool CanLinkExternalContent
);

public record ExtensionComponentView(
    [property: JsonPropertyName("viewer_url")] string ViewerUrl,
    [property: JsonPropertyName("aspect_width")] int AspectWidth,
    [property: JsonPropertyName("aspect_height")] int AspectHeight,
    [property: JsonPropertyName("aspect_ratio_x")] int AspectRatioX,
    [property: JsonPropertyName("aspect_ratio_y")] int AspectRatioY,
    [property: JsonPropertyName("autoscale")] bool Autoscale,
    [property: JsonPropertyName("scale_pixels")] int ScalePixels,
    [property: JsonPropertyName("target_height")] int TargetHeight,
    [property: JsonPropertyName("size")] int Size,
    [property: JsonPropertyName("zoom")] bool Zoom,
    [property: JsonPropertyName("zoom_pixels")] int ZoomPixels,
    [property: JsonPropertyName("can_link_external_content")] bool CanLinkExternalContent
);

public record ExtensionConfigView(
    [property: JsonPropertyName("viewer_url")] string ViewerUrl,
    [property: JsonPropertyName("can_link_external_content")] bool CanLinkExternalContent
);
