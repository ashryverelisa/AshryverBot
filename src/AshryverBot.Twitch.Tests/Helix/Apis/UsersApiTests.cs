using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Models.Users.GetUserActiveExtensions;
using AshryverBot.Twitch.Helix.Models.Users.GetUserBlockList;
using AshryverBot.Twitch.Helix.Models.Users.GetUserExtensions;
using AshryverBot.Twitch.Helix.Models.Users.GetUsers;
using AshryverBot.Twitch.Helix.Models.Users.UpdateUser;
using AshryverBot.Twitch.Helix.Models.Users.UpdateUserExtensions;
using AshryverBot.Twitch.Tests.TestSupport;
using NSubstitute;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis;

public class UsersApiTests
{
    private readonly ITwitchClient _client = Substitute.For<ITwitchClient>();
    private readonly UsersApi _api;
    public UsersApiTests() => _api = new UsersApi(_client);

    private static readonly ActiveExtensionsConfiguration EmptyConfig =
        new(new Dictionary<string, ActiveExtensionSlot>(),
            new Dictionary<string, ActiveExtensionSlot>(),
            new Dictionary<string, ActiveExtensionSlot>());

    [Fact]
    public async Task GetUsersAsync_no_params_sends_empty_query()
    {
        _client.GetAsync<GetUsersResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetUsersResponse([]));

        await _api.GetUsersAsync("tok");

        await _client.Received(1).GetAsync<GetUsersResponse>(
            "users", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.TotalCount() == 0),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetUsersAsync_ids_and_logins_multi()
    {
        _client.GetAsync<GetUsersResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetUsersResponse([]));

        await _api.GetUsersAsync("tok", ids: ["1", "2"], logins: ["a", "b"]);

        await _client.Received(1).GetAsync<GetUsersResponse>(
            "users", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.CountKey("id") == 2 && q.CountKey("login") == 2),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateUserAsync_no_description_sends_empty_query()
    {
        _client.PutAsync<UpdateUserResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new UpdateUserResponse([]));

        await _api.UpdateUserAsync("tok");

        await _client.Received(1).PutAsync<UpdateUserResponse>(
            "users", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.TotalCount() == 0),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateUserAsync_with_description()
    {
        _client.PutAsync<UpdateUserResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new UpdateUserResponse([]));

        await _api.UpdateUserAsync("tok", "hello world");

        await _client.Received(1).PutAsync<UpdateUserResponse>(
            "users", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.Has("description", "hello world")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetUserBlockListAsync_required_and_paging()
    {
        _client.GetAsync<GetUserBlockListResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetUserBlockListResponse([], null));

        await _api.GetUserBlockListAsync("tok", "bc", 50, "cur");

        await _client.Received(1).GetAsync<GetUserBlockListResponse>(
            "users/blocks", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("broadcaster_id", "bc") && q.Has("first", "50") && q.Has("after", "cur")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task BlockUserAsync_uses_PUT_users_blocks_with_target_only()
    {
        await _api.BlockUserAsync("tok", "u");

        await _client.Received(1).PutAsync(
            "users/blocks", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("target_user_id", "u") && q.HasNoKey("source_context") && q.HasNoKey("reason")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task BlockUserAsync_with_source_and_reason()
    {
        await _api.BlockUserAsync("tok", "u", "chat", "spam");

        await _client.Received(1).PutAsync(
            "users/blocks", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("target_user_id", "u") && q.Has("source_context", "chat") && q.Has("reason", "spam")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UnblockUserAsync_uses_DELETE_users_blocks()
    {
        await _api.UnblockUserAsync("tok", "u");

        await _client.Received(1).DeleteAsync(
            "users/blocks", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q =>
                q.Has("target_user_id", "u") && q.TotalCount() == 1),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetUserExtensionsAsync_uses_users_extensions_list_no_query()
    {
        _client.GetAsync<GetUserExtensionsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetUserExtensionsResponse([]));

        await _api.GetUserExtensionsAsync("tok");

        await _client.Received(1).GetAsync<GetUserExtensionsResponse>(
            "users/extensions/list", "tok", null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetUserActiveExtensionsAsync_with_user_id()
    {
        _client.GetAsync<GetUserActiveExtensionsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetUserActiveExtensionsResponse(EmptyConfig));

        await _api.GetUserActiveExtensionsAsync("tok", "u");

        await _client.Received(1).GetAsync<GetUserActiveExtensionsResponse>(
            "users/extensions", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.Has("user_id", "u")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetUserActiveExtensionsAsync_without_user_id_sends_empty_query()
    {
        _client.GetAsync<GetUserActiveExtensionsResponse>(null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new GetUserActiveExtensionsResponse(EmptyConfig));

        await _api.GetUserActiveExtensionsAsync("tok");

        await _client.Received(1).GetAsync<GetUserActiveExtensionsResponse>(
            "users/extensions", "tok",
            Arg.Is<IReadOnlyList<KeyValuePair<string, string>>>(q => q.TotalCount() == 0),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateUserExtensionsAsync_forwards_body_via_PUT()
    {
        _client.PutAsync<UpdateUserExtensionsRequest, UpdateUserExtensionsResponse>(null!, null!, null!, null, CancellationToken.None)
            .ReturnsForAnyArgs(new UpdateUserExtensionsResponse(EmptyConfig));

        var body = new UpdateUserExtensionsRequest(EmptyConfig);
        await _api.UpdateUserExtensionsAsync("tok", body);

        await _client.Received(1).PutAsync<UpdateUserExtensionsRequest, UpdateUserExtensionsResponse>(
            "users/extensions", "tok", body, null, Arg.Any<CancellationToken>());
    }
}
