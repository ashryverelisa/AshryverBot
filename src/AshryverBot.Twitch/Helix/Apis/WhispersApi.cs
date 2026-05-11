using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Models.Whispers.SendWhisper;

namespace AshryverBot.Twitch.Helix.Apis;

public class WhispersApi(ITwitchClient client) : IWhispersApi
{
    public Task SendWhisperAsync(
        string accessToken,
        string fromUserId,
        string toUserId,
        SendWhisperRequest body,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("from_user_id", fromUserId),
            new("to_user_id", toUserId),
        };
        return client.PostAsync("whispers", accessToken, body, query, cancellationToken);
    }
}
