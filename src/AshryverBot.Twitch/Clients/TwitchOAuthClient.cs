using System.Net.Http.Json;
using AshryverBot.Twitch.Auth;
using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AshryverBot.Twitch.Clients;

public class TwitchOAuthClient(
    HttpClient httpClient,
    IOptions<TwitchOptions> options,
    ILogger<TwitchOAuthClient> logger) : ITwitchOAuthClient
{
    private readonly TwitchOptions _options = options.Value;

    public async Task<RefreshResponse> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken);

        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["client_id"] = _options.ClientId,
            ["client_secret"] = _options.ClientSecret,
            ["grant_type"] = "refresh_token",
            ["refresh_token"] = refreshToken,
        });

        using var response = await httpClient.PostAsync("token", content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogWarning("Twitch token refresh failed with {StatusCode}: {Body}", response.StatusCode, body);
            response.EnsureSuccessStatusCode();
        }

        var refreshed = await response.Content.ReadFromJsonAsync<RefreshResponse>(cancellationToken)
            ?? throw new InvalidOperationException("Twitch returned an empty refresh response.");

        return refreshed;
    }

    public async Task<bool> RevokeAsync(string accessToken, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);

        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["client_id"] = _options.ClientId,
            ["token"] = accessToken,
        });

        using var response = await httpClient.PostAsync("revoke", content, cancellationToken);
        return response.IsSuccessStatusCode;
    }
}
