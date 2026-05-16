namespace AshryverBot.Twitch.EventSub.WebSocket;

public class EventSubWebSocketOptions
{
    public const string SectionName = "EventSub";

    public string Url { get; set; } = "wss://eventsub.wss.twitch.tv/ws";
}
