using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Chat.GetChannelChatBadges;
using AshryverBot.Twitch.Helix.Models.Chat.GetChannelEmotes;
using AshryverBot.Twitch.Helix.Models.Chat.GetChatSettings;
using AshryverBot.Twitch.Helix.Models.Chat.GetChatters;
using AshryverBot.Twitch.Helix.Models.Chat.GetEmoteSets;
using AshryverBot.Twitch.Helix.Models.Chat.GetGlobalChatBadges;
using AshryverBot.Twitch.Helix.Models.Chat.GetGlobalEmotes;
using AshryverBot.Twitch.Helix.Models.Chat.GetSharedChatSession;
using AshryverBot.Twitch.Helix.Models.Chat.GetUserChatColor;
using AshryverBot.Twitch.Helix.Models.Chat.GetUserEmotes;
using AshryverBot.Twitch.Helix.Models.Chat.SendChatAnnouncement;
using AshryverBot.Twitch.Helix.Models.Chat.SendChatMessage;
using AshryverBot.Twitch.Helix.Models.Chat.UpdateChatSettings;
using AshryverBot.Twitch.Helix.Models.Common;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class ChatApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly ChatApi _api;
    public ChatApiTests() => _api = new ChatApi(_client);

    [Fact]
    public async Task GetChannelChatBadgesAsync_calls_chat_badges_with_broadcaster()
    {
        _client.GetAsync<GetChannelChatBadgesResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetChannelChatBadgesResponse([]));

        await _api.GetChannelChatBadgesAsync("tok", "bc");

        await _client.Received(1).GetAsync<GetChannelChatBadgesResponse>(
            "chat/badges", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetGlobalChatBadgesAsync_calls_chat_badges_global_no_query()
    {
        _client.GetAsync<GetGlobalChatBadgesResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetGlobalChatBadgesResponse([]));

        await _api.GetGlobalChatBadgesAsync("tok");

        await _client.Received(1).GetAsync<GetGlobalChatBadgesResponse>(
            "chat/badges/global", "tok", null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetChannelEmotesAsync_calls_chat_emotes()
    {
        _client.GetAsync<GetChannelEmotesResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetChannelEmotesResponse([], ""));

        await _api.GetChannelEmotesAsync("tok", "bc");

        await _client.Received(1).GetAsync<GetChannelEmotesResponse>(
            "chat/emotes", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetGlobalEmotesAsync_calls_chat_emotes_global_no_query()
    {
        _client.GetAsync<GetGlobalEmotesResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetGlobalEmotesResponse([], ""));

        await _api.GetGlobalEmotesAsync("tok");

        await _client.Received(1).GetAsync<GetGlobalEmotesResponse>(
            "chat/emotes/global", "tok", null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetEmoteSetsAsync_passes_multiple_emote_set_ids()
    {
        _client.GetAsync<GetEmoteSetsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetEmoteSetsResponse([], ""));

        await _api.GetEmoteSetsAsync("tok", ["s1", "s2", "s3"]);

        await _client.Received(1).GetAsync<GetEmoteSetsResponse>(
            "chat/emotes/set", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.CountKey("emote_set_id") == 3),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetUserEmotesAsync_required_only()
    {
        _client.GetAsync<GetUserEmotesResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetUserEmotesResponse([], "", null));

        await _api.GetUserEmotesAsync("tok", "u");

        await _client.Received(1).GetAsync<GetUserEmotesResponse>(
            "chat/emotes/user", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("user_id", "u") && q.HasNoKey("broadcaster_id") && q.HasNoKey("after")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetUserEmotesAsync_with_filters()
    {
        _client.GetAsync<GetUserEmotesResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetUserEmotesResponse([], "", null));

        await _api.GetUserEmotesAsync("tok", "u", "bc", "cur");

        await _client.Received(1).GetAsync<GetUserEmotesResponse>(
            "chat/emotes/user", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("user_id", "u") && q.Has("broadcaster_id", "bc") && q.Has("after", "cur")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetChatSettingsAsync_required_only()
    {
        _client.GetAsync<GetChatSettingsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetChatSettingsResponse([]));

        await _api.GetChatSettingsAsync("tok", "bc");

        await _client.Received(1).GetAsync<GetChatSettingsResponse>(
            "chat/settings", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.HasNoKey("moderator_id")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetChatSettingsAsync_with_moderator_id()
    {
        _client.GetAsync<GetChatSettingsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetChatSettingsResponse([]));

        await _api.GetChatSettingsAsync("tok", "bc", "mod");

        await _client.Received(1).GetAsync<GetChatSettingsResponse>(
            "chat/settings", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateChatSettingsAsync_uses_PATCH_with_body()
    {
        _client.PatchAsync<UpdateChatSettingsRequest, UpdateChatSettingsResponse>(
            null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new UpdateChatSettingsResponse([]));

        var body = new UpdateChatSettingsRequest { SlowMode = true };
        await _api.UpdateChatSettingsAsync("tok", "bc", "mod", body);

        await _client.Received(1).PatchAsync<UpdateChatSettingsRequest, UpdateChatSettingsResponse>(
            "chat/settings", "tok", body,
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SendChatAnnouncementAsync_uses_POST_with_query_and_body()
    {
        var body = new SendChatAnnouncementRequest("hello", "blue");
        await _api.SendChatAnnouncementAsync("tok", "bc", "mod", body);

        await _client.Received(1).PostAsync(
            "chat/announcements", "tok", body,
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SendShoutoutAsync_uses_POST_shoutouts_with_three_ids()
    {
        await _api.SendShoutoutAsync("tok", "from", "to", "mod");

        await _client.Received(1).PostAsync(
            "chat/shoutouts", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("from_broadcaster_id", "from") && q.Has("to_broadcaster_id", "to")
                && q.Has("moderator_id", "mod") && q.TotalCount() == 3),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SendChatMessageAsync_uses_POST_messages_with_body_no_query()
    {
        _client.PostAsync<SendChatMessageRequest, SendChatMessageResponse>(
            null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new SendChatMessageResponse([]));

        var body = new SendChatMessageRequest { BroadcasterId = "bc", SenderId = "s", Message = "hi" };
        await _api.SendChatMessageAsync("tok", body);

        await _client.Received(1).PostAsync<SendChatMessageRequest, SendChatMessageResponse>(
            "chat/messages", "tok", body, null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetUserChatColorAsync_passes_multiple_user_ids()
    {
        _client.GetAsync<GetUserChatColorResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetUserChatColorResponse([]));

        await _api.GetUserChatColorAsync("tok", ["u1", "u2"]);

        await _client.Received(1).GetAsync<GetUserChatColorResponse>(
            "chat/color", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.CountKey("user_id") == 2),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateUserChatColorAsync_uses_PUT_color()
    {
        await _api.UpdateUserChatColorAsync("tok", "u", "blue");

        await _client.Received(1).PutAsync(
            "chat/color", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("user_id", "u") && q.Has("color", "blue") && q.TotalCount() == 2),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetChattersAsync_required_only()
    {
        _client.GetAsync<GetChattersResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetChattersResponse([], new Pagination(""), 0));

        await _api.GetChattersAsync("tok", "bc", "mod");

        await _client.Received(1).GetAsync<GetChattersResponse>(
            "chat/chatters", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod")
                && q.HasNoKey("first") && q.HasNoKey("after")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetChattersAsync_with_paging()
    {
        _client.GetAsync<GetChattersResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetChattersResponse([], new Pagination(""), 0));

        await _api.GetChattersAsync("tok", "bc", "mod", 1000, "cur");

        await _client.Received(1).GetAsync<GetChattersResponse>(
            "chat/chatters", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("first", "1000") && q.Has("after", "cur")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetSharedChatSessionAsync_calls_shared_chat_session()
    {
        _client.GetAsync<GetSharedChatSessionResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetSharedChatSessionResponse([]));

        await _api.GetSharedChatSessionAsync("tok", "bc");

        await _client.Received(1).GetAsync<GetSharedChatSessionResponse>(
            "chat/shared_chat/session", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }
}
