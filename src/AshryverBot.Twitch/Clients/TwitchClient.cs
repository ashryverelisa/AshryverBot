using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AshryverBot.Twitch.Clients;

public class TwitchClient(
    HttpClient httpClient,
    IOptions<TwitchOptions> options,
    ILogger<TwitchClient> logger) : ITwitchClient
{
    private static readonly HttpMethod _patch = HttpMethod.Patch;
    private readonly TwitchOptions _options = options.Value;

    public Task<TResponse> GetAsync<TResponse>(
        string resourceUrl,
        string accessToken,
        IReadOnlyList<KeyValuePair<string, string>>? queryParameters = null,
        CancellationToken cancellationToken = default)
        => SendForResponseAsync<TResponse>(HttpMethod.Get, resourceUrl, accessToken, payload: null, queryParameters, cancellationToken);

    public Task<TResponse> PostAsync<TRequest, TResponse>(
        string resourceUrl,
        string accessToken,
        TRequest payload,
        IReadOnlyList<KeyValuePair<string, string>>? queryParameters = null,
        CancellationToken cancellationToken = default)
        => SendForResponseAsync<TResponse>(HttpMethod.Post, resourceUrl, accessToken, JsonContent.Create(payload), queryParameters, cancellationToken);

    public Task<TResponse> PostAsync<TResponse>(
        string resourceUrl,
        string accessToken,
        IReadOnlyList<KeyValuePair<string, string>>? queryParameters = null,
        CancellationToken cancellationToken = default)
        => SendForResponseAsync<TResponse>(HttpMethod.Post, resourceUrl, accessToken, payload: null, queryParameters, cancellationToken);

    public Task PostAsync(
        string resourceUrl,
        string accessToken,
        IReadOnlyList<KeyValuePair<string, string>>? queryParameters = null,
        CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, resourceUrl, accessToken, payload: null, queryParameters, cancellationToken);

    public Task<TResponse> PatchAsync<TRequest, TResponse>(
        string resourceUrl,
        string accessToken,
        TRequest payload,
        IReadOnlyList<KeyValuePair<string, string>>? queryParameters = null,
        CancellationToken cancellationToken = default)
        => SendForResponseAsync<TResponse>(_patch, resourceUrl, accessToken, JsonContent.Create(payload), queryParameters, cancellationToken);

    public Task PatchAsync<TRequest>(
        string resourceUrl,
        string accessToken,
        TRequest payload,
        IReadOnlyList<KeyValuePair<string, string>>? queryParameters = null,
        CancellationToken cancellationToken = default)
        => SendAsync(_patch, resourceUrl, accessToken, JsonContent.Create(payload), queryParameters, cancellationToken);

    public Task<TResponse> PutAsync<TRequest, TResponse>(
        string resourceUrl,
        string accessToken,
        TRequest payload,
        IReadOnlyList<KeyValuePair<string, string>>? queryParameters = null,
        CancellationToken cancellationToken = default)
        => SendForResponseAsync<TResponse>(HttpMethod.Put, resourceUrl, accessToken, JsonContent.Create(payload), queryParameters, cancellationToken);

    public Task PutAsync<TRequest>(
        string resourceUrl,
        string accessToken,
        TRequest payload,
        IReadOnlyList<KeyValuePair<string, string>>? queryParameters = null,
        CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, resourceUrl, accessToken, JsonContent.Create(payload), queryParameters, cancellationToken);

    public Task DeleteAsync(
        string resourceUrl,
        string accessToken,
        IReadOnlyList<KeyValuePair<string, string>>? queryParameters = null,
        CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, resourceUrl, accessToken, payload: null, queryParameters, cancellationToken);

    private async Task<TResponse> SendForResponseAsync<TResponse>(
        HttpMethod method,
        string resourceUrl,
        string accessToken,
        HttpContent? payload,
        IReadOnlyList<KeyValuePair<string, string>>? queryParameters,
        CancellationToken cancellationToken)
    {
        using var response = await SendCoreAsync(method, resourceUrl, accessToken, payload, queryParameters, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NoContent)
            return default!;

        var body = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken)
            ?? throw new InvalidOperationException($"Twitch returned an empty response for {method} {resourceUrl}.");

        return body;
    }

    private async Task SendAsync(
        HttpMethod method,
        string resourceUrl,
        string accessToken,
        HttpContent? payload,
        IReadOnlyList<KeyValuePair<string, string>>? queryParameters,
        CancellationToken cancellationToken)
    {
        using var response = await SendCoreAsync(method, resourceUrl, accessToken, payload, queryParameters, cancellationToken);
    }

    private async Task<HttpResponseMessage> SendCoreAsync(
        HttpMethod method,
        string resourceUrl,
        string accessToken,
        HttpContent? payload,
        IReadOnlyList<KeyValuePair<string, string>>? queryParameters,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(resourceUrl);
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);

        var requestUri = BuildRequestUri(resourceUrl, queryParameters);
        using var request = new HttpRequestMessage(method, requestUri);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.Add("Client-Id", _options.ClientId);

        if (payload is not null)
            request.Content = payload;

        var response = await httpClient.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode) return response;

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        logger.LogWarning("Twitch Helix {Method} {Uri} failed with {StatusCode}: {Body}",
            method, requestUri, response.StatusCode, body);
        response.EnsureSuccessStatusCode();

        return response;
    }

    private static string BuildRequestUri(string resourceUrl, IReadOnlyList<KeyValuePair<string, string>>? queryParameters)
    {
        var path = resourceUrl.TrimStart('/');

        if (queryParameters is null || queryParameters.Count == 0)
            return path;

        var builder = new StringBuilder(path);
        var first = true;
        foreach (var (key, value) in queryParameters)
        {
            builder.Append(first ? '?' : '&');
            builder.Append(Uri.EscapeDataString(key));
            builder.Append('=');
            builder.Append(Uri.EscapeDataString(value));
            first = false;
        }

        return builder.ToString();
    }
}