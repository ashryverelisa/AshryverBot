using AshryverBot.Twitch.Clients;
using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AshryverBot.Twitch;

public static class DependencyInjection
{
    public static IServiceCollection AddTwitchClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<TwitchOptions>()
            .Bind(configuration.GetSection(TwitchOptions.SectionName))
            .Validate(o => !string.IsNullOrWhiteSpace(o.ClientId), "Twitch:ClientId missing.")
            .Validate(o => !string.IsNullOrWhiteSpace(o.ClientSecret), "Twitch:ClientSecret missing.")
            .ValidateOnStart();

        services.AddHttpClient<ITwitchOAuthClient, TwitchOAuthClient>(client =>
        {
            client.BaseAddress = new Uri(Constants.BaseAuth.TrimEnd('/') + "/");
        });

        services.AddHttpClient<ITwitchClient, TwitchClient>(client =>
        {
            client.BaseAddress = new Uri(Constants.BaseHelix.TrimEnd('/') + "/");
        });

        return services;
    }
}
