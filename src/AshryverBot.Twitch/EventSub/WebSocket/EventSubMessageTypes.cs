namespace AshryverBot.Twitch.EventSub.WebSocket;

public static class EventSubMessageTypes
{
    public const string SessionWelcome = "session_welcome";
    public const string SessionKeepalive = "session_keepalive";
    public const string SessionReconnect = "session_reconnect";
    public const string Notification = "notification";
    public const string Revocation = "revocation";
}
