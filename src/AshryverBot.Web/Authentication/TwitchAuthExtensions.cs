using System.Security.Claims;
using AshryverBot.Database.Repositories;
using AshryverBot.Infrastructure.Twitch.Tokens;
using AshryverBot.Twitch.Configuration;
using AspNet.Security.OAuth.Twitch;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;

namespace AshryverBot.Web.Authentication;

public static class TwitchAuthExtensions
{
    public static IServiceCollection AddTwitchLogin(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("Twitch");
        var clientId = section["ClientId"] ?? throw new InvalidOperationException("Configuration 'Twitch:ClientId' missing.");
        var clientSecret = section["ClientSecret"] ?? throw new InvalidOperationException("Configuration 'Twitch:ClientSecret' missing.");

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = TwitchAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/login";
                options.ExpireTimeSpan = TimeSpan.FromDays(14);
                options.SlidingExpiration = true;
                options.Cookie.Name = "ashryver.auth";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Events.OnValidatePrincipal = OnValidatePrincipalAsync;
            })
            .AddTwitch(options =>
            {
                options.ClientId = clientId;
                options.ClientSecret = clientSecret;
                options.CallbackPath = "/signin-twitch";
                options.SaveTokens = true;
                options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
                options.CorrelationCookie.SameSite = SameSiteMode.Lax;

                options.Scope.Clear();
                foreach (var scope in TwitchScopes.All)
                    options.Scope.Add(scope);

                options.Events.OnCreatingTicket = OnCreatingTicketAsync;
            });

        services.AddAuthorization();
        services.AddCascadingAuthenticationState();

        return services;
    }

    private static async Task OnCreatingTicketAsync(OAuthCreatingTicketContext context)
    {
        var identity = context.Identity ?? throw new InvalidOperationException("ClaimsIdentity missing on OAuth ticket.");
        var twitchUserId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(twitchUserId))
            return;

        var login = identity.FindFirst(ClaimTypes.Name)?.Value
                    ?? identity.FindFirst("urn:twitch:login")?.Value
                    ?? string.Empty;

        var displayName = identity.FindFirst("urn:twitch:displayname")?.Value;
        var email = identity.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrWhiteSpace(context.AccessToken) || string.IsNullOrWhiteSpace(context.RefreshToken))
            return;

        var services = context.HttpContext.RequestServices;
        var timeProvider = services.GetRequiredService<TimeProvider>();
        var repository = services.GetRequiredService<ITwitchTokenRepository>();
        var twitchOptions = services.GetRequiredService<IOptions<TwitchOptions>>().Value;

        var expiresAt = context.ExpiresIn is { TotalSeconds: > 0 } expiresIn
            ? timeProvider.GetUtcNow().Add(expiresIn)
            : timeProvider.GetUtcNow().AddHours(4);

        var configuredBotUserId = twitchOptions.BotUserId;
        var isBot = !string.IsNullOrWhiteSpace(configuredBotUserId)
            && string.Equals(configuredBotUserId, twitchUserId, StringComparison.Ordinal);

        var info = new TwitchTokenInfo(
            twitchUserId,
            login,
            displayName,
            email,
            context.AccessToken,
            context.RefreshToken,
            expiresAt,
            ExtractScopes(context),
            IsBotAccount: isBot);

        await repository.UpsertAsync(info, timeProvider.GetUtcNow(), context.HttpContext.RequestAborted);
    }

    private static IReadOnlyCollection<string> ExtractScopes(OAuthCreatingTicketContext context)
    {
        var response = context.TokenResponse?.Response;

        if (response is null || !response.RootElement.TryGetProperty("scope", out var scope))
            return [];

        return scope.ValueKind switch
        {
            System.Text.Json.JsonValueKind.Array => scope.EnumerateArray()
                .Select(e => e.GetString())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s!)
                .ToArray(),
            System.Text.Json.JsonValueKind.String => (scope.GetString() ?? string.Empty)
                .Split(' ', StringSplitOptions.RemoveEmptyEntries),
            _ => [],
        };
    }

    private static async Task OnValidatePrincipalAsync(CookieValidatePrincipalContext context)
    {
        var twitchUserId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(twitchUserId))
            return;

        var refresher = context.HttpContext.RequestServices.GetRequiredService<ITwitchTokenRefresher>();
        var token = await refresher.GetValidAsync(twitchUserId, context.HttpContext.RequestAborted);

        if (token is null)
        {
            context.RejectPrincipal();
            await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
