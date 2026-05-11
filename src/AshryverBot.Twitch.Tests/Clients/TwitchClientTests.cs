using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using AshryverBot.Twitch.Clients;
using AshryverBot.Twitch.Configuration;
using AshryverBot.Twitch.Tests.TestSupport;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xunit;

namespace AshryverBot.Twitch.Tests.Clients;

public class TwitchClientTests
{
    private record TestResponse([property: JsonPropertyName("data")] IReadOnlyCollection<string> Data);
    private record TestBody([property: JsonPropertyName("message")] string Message);

    private static (TwitchClient client, MockHttpMessageHandler handler) Create(
        Func<HttpRequestMessage, HttpResponseMessage>? respond = null)
    {
        var handler = new MockHttpMessageHandler((req, _) =>
        {
            var response = respond?.Invoke(req)
                ?? new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(new TestResponse(["ok"])),
                };
            return Task.FromResult(response);
        });

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.twitch.tv/helix/"),
        };
        var options = Options.Create(new TwitchOptions { ClientId = "test-client" });
        var client = new TwitchClient(httpClient, options, NullLogger<TwitchClient>.Instance);
        return (client, handler);
    }

    [Fact]
    public async Task GetAsync_appends_query_parameters_to_url()
    {
        var (client, handler) = Create();
        await client.GetAsync<TestResponse>("users", "tok",
        [
            new KeyValuePair<string, string>("id", "1"),
                new KeyValuePair<string, string>("id", "2"),
                new KeyValuePair<string, string>("login", "alice")
        ]);

        var req = Assert.Single(handler.Requests);
        Assert.Equal("https://api.twitch.tv/helix/users?id=1&id=2&login=alice",
            req.RequestUri!.AbsoluteUri);
    }

    [Fact]
    public async Task GetAsync_omits_query_string_when_no_parameters()
    {
        var (client, handler) = Create();
        await client.GetAsync<TestResponse>("users", "tok");

        Assert.Equal("https://api.twitch.tv/helix/users", handler.Requests[0].RequestUri!.AbsoluteUri);
    }

    [Fact]
    public async Task GetAsync_strips_leading_slash_from_resource()
    {
        var (client, handler) = Create();
        await client.GetAsync<TestResponse>("/users", "tok");
        Assert.Equal("https://api.twitch.tv/helix/users", handler.Requests[0].RequestUri!.AbsoluteUri);
    }

    [Fact]
    public async Task GetAsync_url_encodes_query_values()
    {
        var (client, handler) = Create();
        await client.GetAsync<TestResponse>("search/channels", "tok",
            [new KeyValuePair<string, string>("query", "hello world&more")]);

        Assert.Equal(
            "https://api.twitch.tv/helix/search/channels?query=hello%20world%26more",
            handler.Requests[0].RequestUri!.AbsoluteUri);
    }

    [Fact]
    public async Task SendCore_sets_bearer_authorization_and_client_id_headers()
    {
        var (client, handler) = Create();
        await client.GetAsync<TestResponse>("users", "abcdef");

        var req = handler.Requests[0];
        Assert.Equal("Bearer", req.Headers.Authorization?.Scheme);
        Assert.Equal("abcdef", req.Headers.Authorization?.Parameter);
        Assert.Equal("test-client", req.Headers.GetValues("Client-Id").Single());
    }

    [Fact]
    public async Task GetAsync_uses_GET_verb()
    {
        var (client, handler) = Create();
        await client.GetAsync<TestResponse>("users", "tok");
        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
    }

    [Fact]
    public async Task PostAsync_with_body_serializes_json_payload()
    {
        var (client, handler) = Create();
        await client.PostAsync<TestBody, TestResponse>("things", "tok", new TestBody("hi"));

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Contains("\"message\":\"hi\"", handler.RequestBodies[0]);
        Assert.Equal("application/json", handler.Requests[0].Content!.Headers.ContentType!.MediaType);
    }

    [Fact]
    public async Task PostAsync_no_body_sends_empty_content()
    {
        var (client, handler) = Create();
        await client.PostAsync<TestResponse>("things", "tok");

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Null(handler.Requests[0].Content);
    }

    [Fact]
    public async Task PostAsync_void_no_body_sends_empty()
    {
        var (client, handler) = Create();
        await client.PostAsync("things", "tok");

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Null(handler.Requests[0].Content);
    }

    [Fact]
    public async Task PostAsync_void_with_body_sends_json()
    {
        var (client, handler) = Create();
        await client.PostAsync("things", "tok", new TestBody("x"));

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Contains("\"message\":\"x\"", handler.RequestBodies[0]);
    }

    [Fact]
    public async Task PatchAsync_with_body_uses_PATCH()
    {
        var (client, handler) = Create();
        await client.PatchAsync<TestBody, TestResponse>("things", "tok", new TestBody("p"));
        Assert.Equal(HttpMethod.Patch, handler.Requests[0].Method);
    }

    [Fact]
    public async Task PatchAsync_void_uses_PATCH()
    {
        var (client, handler) = Create();
        await client.PatchAsync("things", "tok", new TestBody("p"));
        Assert.Equal(HttpMethod.Patch, handler.Requests[0].Method);
    }

    [Fact]
    public async Task PutAsync_with_body_uses_PUT()
    {
        var (client, handler) = Create();
        await client.PutAsync<TestBody, TestResponse>("things", "tok", new TestBody("p"));
        Assert.Equal(HttpMethod.Put, handler.Requests[0].Method);
    }

    [Fact]
    public async Task PutAsync_no_body_uses_PUT()
    {
        var (client, handler) = Create();
        await client.PutAsync("things", "tok");
        Assert.Equal(HttpMethod.Put, handler.Requests[0].Method);
        Assert.Null(handler.Requests[0].Content);
    }

    [Fact]
    public async Task PutAsync_response_only_uses_PUT_and_returns_body()
    {
        var (client, handler) = Create();
        var result = await client.PutAsync<TestResponse>("things", "tok");
        Assert.Equal(HttpMethod.Put, handler.Requests[0].Method);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeleteAsync_uses_DELETE()
    {
        var (client, handler) = Create();
        await client.DeleteAsync("things", "tok");
        Assert.Equal(HttpMethod.Delete, handler.Requests[0].Method);
    }

    [Fact]
    public async Task DeleteAsync_with_response_uses_DELETE_and_returns_body()
    {
        var (client, handler) = Create();
        var result = await client.DeleteAsync<TestResponse>("things", "tok");
        Assert.Equal(HttpMethod.Delete, handler.Requests[0].Method);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAsync_returns_deserialized_body()
    {
        var (client, _) = Create();
        var result = await client.GetAsync<TestResponse>("users", "tok");
        Assert.Equal(["ok"], result.Data);
    }

    [Fact]
    public async Task SendForResponse_returns_default_on_204_NoContent()
    {
        var (client, _) = Create(_ => new HttpResponseMessage(HttpStatusCode.NoContent));
        var result = await client.GetAsync<TestResponse>("users", "tok");
        Assert.Null(result);
    }

    [Fact]
    public async Task SendForResponse_throws_on_empty_OK_body()
    {
        var (client, _) = Create(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null", System.Text.Encoding.UTF8, "application/json"),
        });
        await Assert.ThrowsAsync<InvalidOperationException>(() => client.GetAsync<TestResponse>("users", "tok"));
    }

    [Fact]
    public async Task NonSuccess_status_throws_HttpRequestException()
    {
        var (client, _) = Create(_ => new HttpResponseMessage(HttpStatusCode.Unauthorized)
        {
            Content = new StringContent("{\"error\":\"Unauthorized\"}"),
        });

        await Assert.ThrowsAsync<HttpRequestException>(() => client.GetAsync<TestResponse>("users", "tok"));
    }

    [Fact]
    public async Task Empty_resourceUrl_throws_ArgumentException()
    {
        var (client, _) = Create();
        await Assert.ThrowsAsync<ArgumentException>(() => client.GetAsync<TestResponse>("", "tok"));
    }

    [Fact]
    public async Task Empty_accessToken_throws_ArgumentException()
    {
        var (client, _) = Create();
        await Assert.ThrowsAsync<ArgumentException>(() => client.GetAsync<TestResponse>("users", ""));
    }

    [Fact]
    public async Task Whitespace_accessToken_throws_ArgumentException()
    {
        var (client, _) = Create();
        await Assert.ThrowsAsync<ArgumentException>(() => client.GetAsync<TestResponse>("users", "   "));
    }

    [Fact]
    public async Task DeleteAsync_with_response_returns_default_on_204()
    {
        var (client, _) = Create(_ => new HttpResponseMessage(HttpStatusCode.NoContent));
        var result = await client.DeleteAsync<TestResponse>("things", "tok");
        Assert.Null(result);
    }
}
