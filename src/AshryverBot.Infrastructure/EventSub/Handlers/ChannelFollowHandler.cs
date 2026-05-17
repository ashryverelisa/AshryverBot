using System.Text.Json;
using AshryverBot.Infrastructure.FollowerStats.Interfaces;
using AshryverBot.Twitch.EventSub.WebSocket;

namespace AshryverBot.Infrastructure.EventSub.Handlers;

internal class ChannelFollowHandler(IFollowerStatsWriter followerStats) : IEventSubHandler
{
    public string SubscriptionType => "channel.follow";

    public string SubscriptionVersion => "2";

    public JsonElement BuildCondition(EventSubSubscriptionContext context)
        => JsonSerializer.SerializeToElement(new
        {
            broadcaster_user_id = context.BroadcasterUserId,
            moderator_user_id = context.BotUserId,
        });

    public Task HandleAsync(EventSubNotification notification, CancellationToken cancellationToken)
    {
        followerStats.RecordFollow();
        return Task.CompletedTask;
    }
}
