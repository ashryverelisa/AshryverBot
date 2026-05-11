using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.GuestStar.CreateGuestStarSession;
using AshryverBot.Twitch.Helix.Models.GuestStar.EndGuestStarSession;
using AshryverBot.Twitch.Helix.Models.GuestStar.GetChannelGuestStarSettings;
using AshryverBot.Twitch.Helix.Models.GuestStar.GetGuestStarInvites;
using AshryverBot.Twitch.Helix.Models.GuestStar.GetGuestStarSession;
using AshryverBot.Twitch.Helix.Models.GuestStar.UpdateChannelGuestStarSettings;
using AshryverBot.Twitch.Helix.Models.GuestStar.UpdateGuestStarSlotSettings;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class GuestStarApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly GuestStarApi _api;
    public GuestStarApiTests() => _api = new GuestStarApi(_client);

    [Fact]
    public async Task GetChannelGuestStarSettingsAsync_calls_channel_settings()
    {
        _client.GetAsync<GetChannelGuestStarSettingsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetChannelGuestStarSettingsResponse([]));

        await _api.GetChannelGuestStarSettingsAsync("tok", "bc", "mod");

        await _client.Received(1).GetAsync<GetChannelGuestStarSettingsResponse>(
            "guest_star/channel_settings", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateChannelGuestStarSettingsAsync_uses_PUT_with_body()
    {
        var body = new UpdateChannelGuestStarSettingsRequest { SlotCount = 5 };
        await _api.UpdateChannelGuestStarSettingsAsync("tok", "bc", body);

        await _client.Received(1).PutAsync(
            "guest_star/channel_settings", "tok", body,
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetGuestStarSessionAsync_calls_session()
    {
        _client.GetAsync<GetGuestStarSessionResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetGuestStarSessionResponse([]));

        await _api.GetGuestStarSessionAsync("tok", "bc", "mod");

        await _client.Received(1).GetAsync<GetGuestStarSessionResponse>(
            "guest_star/session", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateGuestStarSessionAsync_uses_POST_session_with_broadcaster()
    {
        _client.PostAsync<CreateGuestStarSessionResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new CreateGuestStarSessionResponse([]));

        await _api.CreateGuestStarSessionAsync("tok", "bc");

        await _client.Received(1).PostAsync<CreateGuestStarSessionResponse>(
            "guest_star/session", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task EndGuestStarSessionAsync_uses_DELETE_with_session_id()
    {
        _client.DeleteAsync<EndGuestStarSessionResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new EndGuestStarSessionResponse([]));

        await _api.EndGuestStarSessionAsync("tok", "bc", "sess");

        await _client.Received(1).DeleteAsync<EndGuestStarSessionResponse>(
            "guest_star/session", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("session_id", "sess")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetGuestStarInvitesAsync_passes_required_ids()
    {
        _client.GetAsync<GetGuestStarInvitesResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetGuestStarInvitesResponse([]));

        await _api.GetGuestStarInvitesAsync("tok", "bc", "mod", "sess");

        await _client.Received(1).GetAsync<GetGuestStarInvitesResponse>(
            "guest_star/invites", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod") && q.Has("session_id", "sess")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SendGuestStarInviteAsync_uses_POST_invites_with_four_ids()
    {
        await _api.SendGuestStarInviteAsync("tok", "bc", "mod", "sess", "g");

        await _client.Received(1).PostAsync(
            "guest_star/invites", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod")
                && q.Has("session_id", "sess") && q.Has("guest_id", "g")
                && q.TotalCount() == 4),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteGuestStarInviteAsync_uses_DELETE_invites()
    {
        await _api.DeleteGuestStarInviteAsync("tok", "bc", "mod", "sess", "g");

        await _client.Received(1).DeleteAsync(
            "guest_star/invites", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod")
                && q.Has("session_id", "sess") && q.Has("guest_id", "g")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AssignGuestStarSlotAsync_uses_POST_slot_with_five_ids()
    {
        await _api.AssignGuestStarSlotAsync("tok", "bc", "mod", "sess", "g", "slot1");

        await _client.Received(1).PostAsync(
            "guest_star/slot", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod")
                && q.Has("session_id", "sess") && q.Has("guest_id", "g") && q.Has("slot_id", "slot1")
                && q.TotalCount() == 5),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateGuestStarSlotAsync_required_only()
    {
        await _api.UpdateGuestStarSlotAsync("tok", "bc", "mod", "sess", "src");

        await _client.Received(1).PatchAsync<object>(
            "guest_star/slot", "tok", Arg.Any<object>(),
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("source_slot_id", "src") && q.HasNoKey("destination_slot_id")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateGuestStarSlotAsync_with_destination()
    {
        await _api.UpdateGuestStarSlotAsync("tok", "bc", "mod", "sess", "src", "dst");

        await _client.Received(1).PatchAsync<object>(
            "guest_star/slot", "tok", Arg.Any<object>(),
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("source_slot_id", "src") && q.Has("destination_slot_id", "dst")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteGuestStarSlotAsync_required_only()
    {
        await _api.DeleteGuestStarSlotAsync("tok", "bc", "mod", "sess", "g", "slot1");

        await _client.Received(1).DeleteAsync(
            "guest_star/slot", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("slot_id", "slot1") && q.HasNoKey("should_reinvite_guest")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteGuestStarSlotAsync_with_reinvite_flag()
    {
        await _api.DeleteGuestStarSlotAsync("tok", "bc", "mod", "sess", "g", "slot1", "true");

        await _client.Received(1).DeleteAsync(
            "guest_star/slot", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.Has("should_reinvite_guest", "true")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateGuestStarSlotSettingsAsync_uses_PATCH_with_body()
    {
        var body = new UpdateGuestStarSlotSettingsRequest { IsAudioEnabled = true };
        await _api.UpdateGuestStarSlotSettingsAsync("tok", "bc", "mod", "sess", "slot1", body);

        await _client.Received(1).PatchAsync(
            "guest_star/slot_settings", "tok", body,
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("moderator_id", "mod")
                && q.Has("session_id", "sess") && q.Has("slot_id", "slot1")),
            Arg.Any<CancellationToken>());
    }
}
