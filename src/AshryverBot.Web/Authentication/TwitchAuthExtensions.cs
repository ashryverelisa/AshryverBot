using AspNet.Security.OAuth.Twitch;
using Microsoft.AspNetCore.Authentication.Cookies;

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
            });

        services.AddAuthorization();
        services.AddCascadingAuthenticationState();

        return services;
    }
}
