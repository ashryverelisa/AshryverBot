using AshryverBot.Twitch.Helix.Models.GuestStar.CreateGuestStarSession;
using AshryverBot.Twitch.Helix.Models.GuestStar.EndGuestStarSession;
using AshryverBot.Twitch.Helix.Models.GuestStar.GetChannelGuestStarSettings;
using AshryverBot.Twitch.Helix.Models.GuestStar.GetGuestStarInvites;
using AshryverBot.Twitch.Helix.Models.GuestStar.GetGuestStarSession;
using AshryverBot.Twitch.Helix.Models.GuestStar.UpdateChannelGuestStarSettings;
using AshryverBot.Twitch.Helix.Models.GuestStar.UpdateGuestStarSlotSettings;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IGuestStarApi
{
    Task<GetChannelGuestStarSettingsResponse> GetChannelGuestStarSettingsAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        CancellationToken cancellationToken = default);

    Task UpdateChannelGuestStarSettingsAsync(
        string accessToken,
        string broadcasterId,
        UpdateChannelGuestStarSettingsRequest body,
        CancellationToken cancellationToken = default);

    Task<GetGuestStarSessionResponse> GetGuestStarSessionAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        CancellationToken cancellationToken = default);

    Task<CreateGuestStarSessionResponse> CreateGuestStarSessionAsync(
        string accessToken,
        string broadcasterId,
        CancellationToken cancellationToken = default);

    Task<EndGuestStarSessionResponse> EndGuestStarSessionAsync(
        string accessToken,
        string broadcasterId,
        string sessionId,
        CancellationToken cancellationToken = default);

    Task<GetGuestStarInvitesResponse> GetGuestStarInvitesAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string sessionId,
        CancellationToken cancellationToken = default);

    Task SendGuestStarInviteAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string sessionId,
        string guestId,
        CancellationToken cancellationToken = default);

    Task DeleteGuestStarInviteAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string sessionId,
        string guestId,
        CancellationToken cancellationToken = default);

    Task AssignGuestStarSlotAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string sessionId,
        string guestId,
        string slotId,
        CancellationToken cancellationToken = default);

    Task UpdateGuestStarSlotAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string sessionId,
        string sourceSlotId,
        string? destinationSlotId = null,
        CancellationToken cancellationToken = default);

    Task DeleteGuestStarSlotAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string sessionId,
        string guestId,
        string slotId,
        string? shouldReinviteGuest = null,
        CancellationToken cancellationToken = default);

    Task UpdateGuestStarSlotSettingsAsync(
        string accessToken,
        string broadcasterId,
        string moderatorId,
        string sessionId,
        string slotId,
        UpdateGuestStarSlotSettingsRequest body,
        CancellationToken cancellationToken = default);
}
