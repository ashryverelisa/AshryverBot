using AshryverBot.Infrastructure.Twitch.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace AshryverBot.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAshryverBotInfrastructure(this IServiceCollection services)
    {
        services.TryAddSingletonTimeProvider();
        services.AddScoped<ITwitchTokenRefresher, TwitchTokenRefresher>();
        return services;
    }

    private static void TryAddSingletonTimeProvider(this IServiceCollection services)
    {
        if (services.Any(d => d.ServiceType == typeof(TimeProvider))) return;
        services.AddSingleton(TimeProvider.System);
    }
}
