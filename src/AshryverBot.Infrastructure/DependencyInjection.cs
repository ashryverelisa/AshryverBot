using AshryverBot.Infrastructure.Chat;
using AshryverBot.Infrastructure.Chat.Commands;
using AshryverBot.Infrastructure.Twitch.Tokens;
using AshryverBot.Infrastructure.Watchtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AshryverBot.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAshryverBotInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.TryAddSingletonTimeProvider();
        services.AddScoped<ITwitchTokenRefresher, TwitchTokenRefresher>();

        services.AddOptions<WatchtimePollerOptions>()
            .Bind(configuration.GetSection(WatchtimePollerOptions.SectionName));

        services.AddScoped<IChatResponder, ChatResponder>();
        services.AddSingleton<IChatCommandDispatcher, ChatCommandDispatcher>();

        services.AddScoped<IChatCommand, WatchtimeCommand>();

        services.AddHostedService<WatchtimePoller>();
        services.AddHostedService<EventSubChatClient>();

        return services;
    }

    private static void TryAddSingletonTimeProvider(this IServiceCollection services)
    {
        if (services.Any(d => d.ServiceType == typeof(TimeProvider))) return;
        services.AddSingleton(TimeProvider.System);
    }
}
