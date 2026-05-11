using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.GuestStar.CreateGuestStarSession;
using AshryverBot.Twitch.Helix.Models.GuestStar.EndGuestStarSession;
using AshryverBot.Twitch.Helix.Models.GuestStar.GetChannelGuestStarSettings;
using AshryverBot.Twitch.Helix.Models.GuestStar.GetGuestStarInvites;
using AshryverBot.Twitch.Helix.Models.GuestStar.GetGuestStarSession;
using AshryverBot.Twitch.Helix.Models.GuestStar.UpdateChannelGuestStarSettings;
using AshryverBot.Twitch.Helix.Models.GuestStar.UpdateGuestStarSlotSettings;

namespace AshryverBot.Twitch.Helix.Apis;

public class GuestStarApi(ITwitchClient client) : IGuestStarApi
{
    public Task<GetChannelGuestStarSettingsResponse> GetChannelGuestStarSettingsAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
        };
        return client.GetAsync<GetChannelGuestStarSettingsResponse>(
            "guest_star/channel_settings", accessToken, query, cancellationToken);
    }

    public Task UpdateChannelGuestStarSettingsAsync(
        string accessToken,
        string broadcasterId,
        UpdateChannelGuestStarSettingsRequest body,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        return client.PutAsync("guest_star/channel_settings", accessToken, body, query, cancellationToken);
    }

    public Task<GetGuestStarSessionResponse> GetGuestStarSessionAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
        };
        return client.GetAsync<GetGuestStarSessionResponse>("guest_star/session", accessToken, query, cancellationToken);
    }

    public Task<CreateGuestStarSessionResponse> CreateGuestStarSessionAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        return client.PostAsync<CreateGuestStarSessionResponse>("guest_star/session", accessToken, query, cancellationToken);
    }

    public Task<EndGuestStarSessionResponse> EndGuestStarSessionAsync(
        string accessToken,
        string broadcasterId,
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("session_id", sessionId),
        };
        return client.DeleteAsync<EndGuestStarSessionResponse>("guest_star/session", accessToken, query, cancellationToken);
    }

    public Task<GetGuestStarInvitesResponse> GetGuestStarInvitesAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
            new("session_id", sessionId),
        };
        return client.GetAsync<GetGuestStarInvitesResponse>("guest_star/invites", accessToken, query, cancellationToken);
    }

    public Task SendGuestStarInviteAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string sessionId,
        string guestId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
            new("session_id", sessionId),
            new("guest_id", guestId),
        };
        return client.PostAsync("guest_star/invites", accessToken, query, cancellationToken);
    }

    public Task DeleteGuestStarInviteAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string sessionId,
        string guestId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
            new("session_id", sessionId),
            new("guest_id", guestId),
        };
        return client.DeleteAsync("guest_star/invites", accessToken, query, cancellationToken);
    }

    public Task AssignGuestStarSlotAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string sessionId,
        string guestId,
        string slotId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
            new("session_id", sessionId),
            new("guest_id", guestId),
            new("slot_id", slotId),
        };
        return client.PostAsync("guest_star/slot", accessToken, query, cancellationToken);
    }

    public Task UpdateGuestStarSlotAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string sessionId,
        string sourceSlotId,
        string? destinationSlotId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
            new("session_id", sessionId),
            new("source_slot_id", sourceSlotId),
        };
        query.AddIfNotNull("destination_slot_id", destinationSlotId);
        return client.PatchAsync<object>("guest_star/slot", accessToken, new object(), query, cancellationToken);
    }

    public Task DeleteGuestStarSlotAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string sessionId,
        string guestId,
        string slotId,
        string? shouldReinviteGuest = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
            new("session_id", sessionId),
            new("guest_id", guestId),
            new("slot_id", slotId),
        };
        query.AddIfNotNull("should_reinvite_guest", shouldReinviteGuest);
        return client.DeleteAsync("guest_star/slot", accessToken, query, cancellationToken);
    }

    public Task UpdateGuestStarSlotSettingsAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string sessionId,
        string slotId,
        UpdateGuestStarSlotSettingsRequest body,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
            new("moderator_id", moderatorId),
            new("session_id", sessionId),
            new("slot_id", slotId),
        };
        return client.PatchAsync("guest_star/slot_settings", accessToken, body, query, cancellationToken);
    }
}
