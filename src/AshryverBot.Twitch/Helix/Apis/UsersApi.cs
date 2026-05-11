using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Helix.Apis.Internal;
using AshryverBot.Twitch.Helix.Models.Users.GetUserActiveExtensions;
using AshryverBot.Twitch.Helix.Models.Users.GetUserBlockList;
using AshryverBot.Twitch.Helix.Models.Users.GetUserExtensions;
using AshryverBot.Twitch.Helix.Models.Users.GetUsers;
using AshryverBot.Twitch.Helix.Models.Users.UpdateUser;
using AshryverBot.Twitch.Helix.Models.Users.UpdateUserExtensions;

namespace AshryverBot.Twitch.Helix.Apis;

public class UsersApi(ITwitchClient client) : IUsersApi
{
    public Task<GetUsersResponse> GetUsersAsync(
        string accessToken,
        IEnumerable<string>? ids = null,
        IEnumerable<string>? logins = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddMany("id", ids);
        query.AddMany("login", logins);
        return client.GetAsync<GetUsersResponse>("users", accessToken, query, cancellationToken);
    }

    public Task<UpdateUserResponse> UpdateUserAsync(
        string accessToken,
        string? description = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddIfNotNull("description", description);
        return client.PutAsync<UpdateUserResponse>("users", accessToken, query, cancellationToken);
    }

    public Task<GetUserBlockListResponse> GetUserBlockListAsync(
        string accessToken,
        string broadcasterId,
        int? first = null,
        string? after = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("broadcaster_id", broadcasterId),
        };
        query.AddIfNotNull("first", first);
        query.AddIfNotNull("after", after);
        return client.GetAsync<GetUserBlockListResponse>("users/blocks", accessToken, query, cancellationToken);
    }

    public Task BlockUserAsync(
        string accessToken,
        string targetUserId,
        string? sourceContext = null,
        string? reason = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("target_user_id", targetUserId),
        };
        query.AddIfNotNull("source_context", sourceContext);
        query.AddIfNotNull("reason", reason);
        return client.PutAsync("users/blocks", accessToken, query, cancellationToken);
    }

    public Task UnblockUserAsync(
        string accessToken,
        string targetUserId,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>
        {
            new("target_user_id", targetUserId),
        };
        return client.DeleteAsync("users/blocks", accessToken, query, cancellationToken);
    }

    public Task<GetUserExtensionsResponse> GetUserExtensionsAsync(
        string accessToken,
        CancellationToken cancellationToken = default)
        => client.GetAsync<GetUserExtensionsResponse>("users/extensions/list", accessToken, queryParameters: null, cancellationToken);

    public Task<GetUserActiveExtensionsResponse> GetUserActiveExtensionsAsync(
        string accessToken,
        string? userId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        query.AddIfNotNull("user_id", userId);
        return client.GetAsync<GetUserActiveExtensionsResponse>("users/extensions", accessToken, query, cancellationToken);
    }

    public Task<UpdateUserExtensionsResponse> UpdateUserExtensionsAsync(
        string accessToken,
        UpdateUserExtensionsRequest body,
        CancellationToken cancellationToken = default)
        => client.PutAsync<UpdateUserExtensionsRequest, UpdateUserExtensionsResponse>("users/extensions", accessToken, body, queryParameters: null, cancellationToken);
}
