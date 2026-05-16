using AshryverBot.Infrastructure.BotStatus;
using AshryverBot.Infrastructure.Chat;
using AshryverBot.Infrastructure.Chat.Commands;
using AshryverBot.Infrastructure.EventSub;
using AshryverBot.Infrastructure.EventSub.Handlers;
using AshryverBot.Infrastructure.StreamStats;
using AshryverBot.Infrastructure.Twitch.Tokens;
using AshryverBot.Infrastructure.Watchtime;
using AshryverBot.Twitch;
using AshryverBot.Twitch.EventSub.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AshryverBot.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddAshryverBotInfrastructure(IConfiguration configuration)
        {
            services.TryAddSingletonTimeProvider();
            services.AddScoped<ITwitchTokenRefresher, TwitchTokenRefresher>();

            services.AddOptions<WatchtimePollerOptions>()
                .Bind(configuration.GetSection(WatchtimePollerOptions.SectionName));

            services.AddScoped<IChatResponder, ChatResponder>();
            services.AddSingleton<IChatCommandDispatcher, ChatCommandDispatcher>();

            services.AddScoped<IChatCommand, WatchtimeCommand>();

            services.AddTwitchEventSubWebSocket(configuration);
            services.AddSingleton<IEventSubAccessTokenProvider, BotEventSubAccessTokenProvider>();
            services.AddScoped<IEventSubHandler, ChannelChatMessageHandler>();

            services.AddSingleton<BotStatusTracker>();
            services.AddSingleton<IBotStatus>(sp => sp.GetRequiredService<BotStatusTracker>());
            services.AddSingleton<IEventSubConnectionObserver>(sp => sp.GetRequiredService<BotStatusTracker>());

            services.AddSingleton<StreamStatsTracker>();
            services.AddSingleton<IStreamStats>(sp => sp.GetRequiredService<StreamStatsTracker>());
            services.AddSingleton<IStreamStatsWriter>(sp => sp.GetRequiredService<StreamStatsTracker>());

            services.AddHostedService<WatchtimePoller>();
            services.AddHostedService<EventSubWebSocketHostedService>();

            return services;
        }

        private void TryAddSingletonTimeProvider()
        {
            if (services.Any(d => d.ServiceType == typeof(TimeProvider))) return;
            services.AddSingleton(TimeProvider.System);
        }
    }
}
