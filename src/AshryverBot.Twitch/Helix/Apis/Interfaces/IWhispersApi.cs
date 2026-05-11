using AshryverBot.Twitch.Helix.Models.Whispers.SendWhisper;

namespace AshryverBot.Twitch.Helix.Apis.Interfaces;

public interface IWhispersApi
{
    Task SendWhisperAsync(
        string accessToken,
        string fromUserId,
        string toUserId,
        SendWhisperRequest body,
        CancellationToken cancellationToken = default);
}
