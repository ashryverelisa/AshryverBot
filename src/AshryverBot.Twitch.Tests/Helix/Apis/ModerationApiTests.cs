using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Moderation.AddBlockedTerm;
using AshryverBot.Twitch.Helix.Models.Moderation.BanUser;
using AshryverBot.Twitch.Helix.Models.Moderation.CheckAutoModStatus;
using AshryverBot.Twitch.Helix.Models.Moderation.GetAutoModSettings;
using AshryverBot.Twitch.Helix.Models.Moderation.GetBannedUsers;
using AshryverBot.Twitch.Helix.Models.Moderation.GetBlockedTerms;
using AshryverBot.Twitch.Helix.Models.Moderation.GetModeratedChannels;
using AshryverBot.Twitch.Helix.Models.Moderation.GetModerators;
using AshryverBot.Twitch.Helix.Models.Moderation.GetShieldModeStatus;
using AshryverBot.Twitch.Helix.Models.Moderation.GetUnbanRequests;
using AshryverBot.Twitch.Helix.Models.Moderation.ManageHeldAutoModMessages;
using AshryverBot.Twitch.Helix.Models.Moderation.ResolveUnbanRequest;
using AshryverBot.Twitch.Helix.Models.Moderation.UpdateAutoModSettings;
using AshryverBot.Twitch.Helix.Models.Moderation.UpdateShieldModeStatus;
using AshryverBot.Twitch.Helix.Models.Moderation.WarnChatUser;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class ModerationApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly ModerationApi _api;
    public ModerationApiTests() => _api = new ModerationApi(_client);

    [Fact]
    public async Task CheckAutoModStatusAsync_uses_POST_enforcements_status()
    {
        _client.PostAsync<CheckAutoModStatusRequest, CheckAutoModStatusResponse>(
            null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new CheckAutoModStatusResponse([]));

        var body = new CheckAutoModStatusRequest([new CheckAutoModStatusMessage("m1", "text")]);
        await _api.CheckAutoModStatusAsync("tok", "bc", body);

        await _client.Received(1).PostAsync<CheckAutoModStatusRequest, CheckAutoModStatusResponse>(
            "moderation/enforcements/status", "tok", body,
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.Has("broadcaster_id", "bc")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ManageHeldAutoModMessagesAsync_uses_POST_no_query()
    {
        var body = new ManageHeldAutoModMessagesRequest("u", "m", "ALLOW");
        await _api.ManageHeldAutoModMessagesAsync("tok", body);

        await _client.Received(1).PostAsync(
            "moderation/automod/message", "tok", body, null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetAutoModSettingsAsync_calls_automod_settings()
    {
        _client.GetAsync<GetAutoModSettingsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetAutoModSettingsResponse([]));

        await _api.GetAutoModSettingsAsync("tok", "bc", "mod");

        await _client.Received(1).GetAsync<GetAutoModSettingsResponse>(
            "moderation/automod/settings", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAutoModSettingsAsync_uses_PUT_with_body()
    {
        _client.PutAsync<UpdateAutoModSettingsRequest, UpdateAutoModSettingsResponse>(
            null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new UpdateAutoModSettingsResponse([]));

        var body = new UpdateAutoModSettingsRequest { OverallLevel = 3 };
        await _api.UpdateAutoModSettingsAsync("tok", "bc", "mod", body);

        await _client.Received(1).PutAsync<UpdateAutoModSettingsRequest, UpdateAutoModSettingsResponse>(
            "moderation/automod/settings", "tok", body,
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetBannedUsersAsync_required_only()
    {
        _client.GetAsync<GetBannedUsersResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetBannedUsersResponse([], null));

        await _api.GetBannedUsersAsync("tok", "bc");

        await _client.Received(1).GetAsync<GetBannedUsersResponse>(
            "moderation/banned", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.HasNoKey("user_id")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetBannedUsersAsync_all_filters()
    {
        _client.GetAsync<GetBannedUsersResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetBannedUsersResponse([], null));

        await _api.GetBannedUsersAsync("tok", "bc", ["u1", "u2"], 50, "a", "b");

        await _client.Received(1).GetAsync<GetBannedUsersResponse>(
            "moderation/banned", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.CountKey("user_id") == 2 && q.Has("first", "50")
                && q.Has("after", "a") && q.Has("before", "b")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task BanUserAsync_uses_POST_bans()
    {
        _client.PostAsync<BanUserRequest, BanUserResponse>(null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new BanUserResponse([]));

        var body = new BanUserRequest(new BanUserPayload("u", 600, "spam"));
        await _api.BanUserAsync("tok", "bc", "mod", body);

        await _client.Received(1).PostAsync<BanUserRequest, BanUserResponse>(
            "moderation/bans", "tok", body,
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UnbanUserAsync_uses_DELETE_bans()
    {
        await _api.UnbanUserAsync("tok", "bc", "mod", "u");

        await _client.Received(1).DeleteAsync(
            "moderation/bans", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod") && q.Has("user_id", "u")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetUnbanRequestsAsync_required_only()
    {
        _client.GetAsync<GetUnbanRequestsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetUnbanRequestsResponse([], null));

        await _api.GetUnbanRequestsAsync("tok", "bc", "mod", "pending");

        await _client.Received(1).GetAsync<GetUnbanRequestsResponse>(
            "moderation/unban_requests", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod")
                && q.Has("status", "pending") && q.HasNoKey("user_id")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetUnbanRequestsAsync_all_filters()
    {
        _client.GetAsync<GetUnbanRequestsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetUnbanRequestsResponse([], null));

        await _api.GetUnbanRequestsAsync("tok", "bc", "mod", "pending", "u", "cur", 25);

        await _client.Received(1).GetAsync<GetUnbanRequestsResponse>(
            "moderation/unban_requests", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("user_id", "u") && q.Has("after", "cur") && q.Has("first", "25")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ResolveUnbanRequestAsync_uses_PATCH_with_query()
    {
        _client.PatchAsync<object, ResolveUnbanRequestResponse>(null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new ResolveUnbanRequestResponse([]));

        await _api.ResolveUnbanRequestAsync("tok", "bc", "mod", "ub", "approved", "ok");

        await _client.Received(1).PatchAsync<object, ResolveUnbanRequestResponse>(
            "moderation/unban_requests", "tok", Arg.Any<object>(),
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod")
                && q.Has("unban_request_id", "ub") && q.Has("status", "approved")
                && q.Has("resolution_text", "ok")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetBlockedTermsAsync_required_only()
    {
        _client.GetAsync<GetBlockedTermsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetBlockedTermsResponse([], null));

        await _api.GetBlockedTermsAsync("tok", "bc", "mod");

        await _client.Received(1).GetAsync<GetBlockedTermsResponse>(
            "moderation/blocked_terms", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod")
                && q.HasNoKey("after") && q.HasNoKey("first")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AddBlockedTermAsync_uses_POST_with_body()
    {
        _client.PostAsync<AddBlockedTermRequest, AddBlockedTermResponse>(
            null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new AddBlockedTermResponse([]));

        var body = new AddBlockedTermRequest("badword");
        await _api.AddBlockedTermAsync("tok", "bc", "mod", body);

        await _client.Received(1).PostAsync<AddBlockedTermRequest, AddBlockedTermResponse>(
            "moderation/blocked_terms", "tok", body,
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RemoveBlockedTermAsync_uses_DELETE_with_id()
    {
        await _api.RemoveBlockedTermAsync("tok", "bc", "mod", "term");

        await _client.Received(1).DeleteAsync(
            "moderation/blocked_terms", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod") && q.Has("id", "term")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteChatMessagesAsync_without_message_id()
    {
        await _api.DeleteChatMessagesAsync("tok", "bc", "mod");

        await _client.Received(1).DeleteAsync(
            "moderation/chat", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod")
                && q.HasNoKey("message_id")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteChatMessagesAsync_with_message_id()
    {
        await _api.DeleteChatMessagesAsync("tok", "bc", "mod", "msg");

        await _client.Received(1).DeleteAsync(
            "moderation/chat", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("message_id", "msg")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetModeratedChannelsAsync_calls_moderation_channels()
    {
        _client.GetAsync<GetModeratedChannelsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetModeratedChannelsResponse([], null));

        await _api.GetModeratedChannelsAsync("tok", "u", "cur", 25);

        await _client.Received(1).GetAsync<GetModeratedChannelsResponse>(
            "moderation/channels", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("user_id", "u") && q.Has("after", "cur") && q.Has("first", "25")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetModeratorsAsync_required_only()
    {
        _client.GetAsync<GetModeratorsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetModeratorsResponse([], null));

        await _api.GetModeratorsAsync("tok", "bc");

        await _client.Received(1).GetAsync<GetModeratorsResponse>(
            "moderation/moderators", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.HasNoKey("user_id")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AddChannelModeratorAsync_uses_POST_moderators()
    {
        await _api.AddChannelModeratorAsync("tok", "bc", "u");

        await _client.Received(1).PostAsync(
            "moderation/moderators", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("user_id", "u")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RemoveChannelModeratorAsync_uses_DELETE_moderators()
    {
        await _api.RemoveChannelModeratorAsync("tok", "bc", "u");

        await _client.Received(1).DeleteAsync(
            "moderation/moderators", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("user_id", "u")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetShieldModeStatusAsync_calls_shield_mode()
    {
        _client.GetAsync<GetShieldModeStatusResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetShieldModeStatusResponse([]));

        await _api.GetShieldModeStatusAsync("tok", "bc", "mod");

        await _client.Received(1).GetAsync<GetShieldModeStatusResponse>(
            "moderation/shield_mode", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateShieldModeStatusAsync_uses_PUT_with_body()
    {
        _client.PutAsync<UpdateShieldModeStatusRequest, UpdateShieldModeStatusResponse>(
            null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new UpdateShieldModeStatusResponse([]));

        var body = new UpdateShieldModeStatusRequest(true);
        await _api.UpdateShieldModeStatusAsync("tok", "bc", "mod", body);

        await _client.Received(1).PutAsync<UpdateShieldModeStatusRequest, UpdateShieldModeStatusResponse>(
            "moderation/shield_mode", "tok", body,
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task WarnChatUserAsync_uses_POST_warnings_with_body()
    {
        _client.PostAsync<WarnChatUserRequest, WarnChatUserResponse>(
            null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new WarnChatUserResponse([]));

        var body = new WarnChatUserRequest(new WarnChatUserPayload("u", "be nice"));
        await _api.WarnChatUserAsync("tok", "bc", "mod", body);

        await _client.Received(1).PostAsync<WarnChatUserRequest, WarnChatUserResponse>(
            "moderation/warnings", "tok", body,
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod")),
            Arg.Any<CancellationToken>());
    }
}
