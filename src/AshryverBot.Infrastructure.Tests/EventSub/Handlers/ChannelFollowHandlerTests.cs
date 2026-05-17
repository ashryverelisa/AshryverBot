using System.Text.Json;
using AshryverBot.Infrastructure.EventSub.Handlers;
using AshryverBot.Infrastructure.FollowerStats.Interfaces;
using AshryverBot.Twitch.EventSub.WebSocket;
using NSubstitute;
using Xunit;

namespace AshryverBot.Infrastructure.Tests.EventSub.Handlers;

public class ChannelFollowHandlerTests
{
    private readonly IFollowerStatsWriter _followerStats = Substitute.For<IFollowerStatsWriter>();

    [Fact]
    public void Subscription_type_and_version_match_twitch_eventsub_v2()
    {
        var handler = new ChannelFollowHandler(_followerStats);

        Assert.Equal("channel.follow", handler.SubscriptionType);
        Assert.Equal("2", handler.SubscriptionVersion);
    }

    [Fact]
    public void BuildCondition_uses_broadcaster_and_moderator_ids()
    {
        var handler = new ChannelFollowHandler(_followerStats);
        var context = new EventSubSubscriptionContext("broadcaster-1", "bot-1");

        var condition = handler.BuildCondition(context);

        Assert.Equal("broadcaster-1", condition.GetProperty("broadcaster_user_id").GetString());
        Assert.Equal("bot-1", condition.GetProperty("moderator_user_id").GetString());
    }

    [Fact]
    public async Task HandleAsync_records_a_follow()
    {
        var handler = new ChannelFollowHandler(_followerStats);
        var notification = new EventSubNotification(
            "channel.follow",
            "2",
            "sub-1",
            JsonDocument.Parse("{}").RootElement);

        await handler.HandleAsync(notification, CancellationToken.None);

        _followerStats.Received(1).RecordFollow();
    }
}
