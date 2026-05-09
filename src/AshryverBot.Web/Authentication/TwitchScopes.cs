namespace AshryverBot.Web.Authentication;

/// <summary>
/// Twitch OAuth scopes requested at login.
/// Reference: https://dev.twitch.tv/docs/authentication/scopes/
/// </summary>
public static class TwitchScopes
{
    public static readonly IReadOnlyList<string> All =
    [
        // User & profile
        "user:read:email",
        "user:read:follows",
        "user:read:subscriptions",
        "user:read:blocked_users",
        "user:manage:blocked_users",
        "user:read:emotes",
        "user:read:moderated_channels",
        "user:edit",
        "user:bot",
        "user:read:chat",
        "user:write:chat",
        "user:manage:chat_color",
        "user:read:whispers",
        "user:manage:whispers",

        // Chat (legacy IRC scopes — still required for TMI/IRC)
        "chat:read",
        "chat:edit",

        // Channel — read
        "channel:read:ads",
        "channel:read:charity",
        "channel:read:editors",
        "channel:read:goals",
        "channel:read:guest_star",
        "channel:read:hype_train",
        "channel:read:polls",
        "channel:read:predictions",
        "channel:read:redemptions",
        "channel:read:stream_key",
        "channel:read:subscriptions",
        "channel:read:vips",
        "channel:bot",

        // Channel — manage / edit
        "channel:edit:commercial",
        "channel:manage:ads",
        "channel:manage:broadcast",
        "channel:manage:extensions",
        "channel:manage:guest_star",
        "channel:manage:moderators",
        "channel:manage:polls",
        "channel:manage:predictions",
        "channel:manage:raids",
        "channel:manage:redemptions",
        "channel:manage:schedule",
        "channel:manage:videos",
        "channel:manage:vips",
        "channel:moderate",

        // Bits
        "bits:read",

        // Moderation
        "moderation:read",
        "moderator:read:followers",
        "moderator:read:chatters",
        "moderator:read:chat_settings",
        "moderator:manage:chat_settings",
        "moderator:read:blocked_terms",
        "moderator:manage:blocked_terms",
        "moderator:read:automod_settings",
        "moderator:manage:automod_settings",
        "moderator:manage:automod",
        "moderator:manage:announcements",
        "moderator:manage:banned_users",
        "moderator:manage:chat_messages",
        "moderator:read:guest_star",
        "moderator:manage:guest_star",
        "moderator:read:moderators",
        "moderator:read:shield_mode",
        "moderator:manage:shield_mode",
        "moderator:read:shoutouts",
        "moderator:manage:shoutouts",
        "moderator:read:suspicious_users",
        "moderator:read:unban_requests",
        "moderator:manage:unban_requests",
        "moderator:read:vips",
        "moderator:read:warnings",
        "moderator:manage:warnings",

        // Clips
        "clips:edit",

        // Analytics
        "analytics:read:games",
        "analytics:read:extensions"
    ];
}
