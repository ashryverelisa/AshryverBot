using AshryverBot.Infrastructure.BotStatus;
using AshryverBot.Infrastructure.BotStatus.Interfaces;
using AshryverBot.Infrastructure.Chat;
using AshryverBot.Infrastructure.Chat.Commands;
using AshryverBot.Infrastructure.Chat.Interfaces;
using AshryverBot.Infrastructure.CommandStats;
using AshryverBot.Infrastructure.CommandStats.Interfaces;
using AshryverBot.Infrastructure.Commands;
using AshryverBot.Infrastructure.Commands.Interfaces;
using AshryverBot.Infrastructure.EventSub;
using AshryverBot.Infrastructure.EventSub.Handlers;
using AshryverBot.Infrastructure.FollowerStats;
using AshryverBot.Infrastructure.FollowerStats.Interfaces;
using AshryverBot.Infrastructure.StreamStats;
using AshryverBot.Infrastructure.StreamStats.Interfaces;
using AshryverBot.Infrastructure.Twitch.Tokens;
using AshryverBot.Infrastructure.Watchtime;
using AshryverBot.Twitch;
using AshryverBot.Twitch.EventSub.WebSocket;
using AshryverBot.Twitch.EventSub.WebSocket.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddScoped<ICommandService, CommandService>();

            services.AddScoped<IChatCommand, WatchtimeCommand>();

            services.AddTwitchEventSubWebSocket(configuration);
            services.AddSingleton<IEventSubAccessTokenProvider, BotEventSubAccessTokenProvider>();
            services.AddScoped<IEventSubHandler, ChannelChatMessageHandler>();
            services.AddScoped<IEventSubHandler, ChannelFollowHandler>();

            services.AddSingleton<BotStatusTracker>();
            services.AddSingleton<IBotStatus>(sp => sp.GetRequiredService<BotStatusTracker>());
            services.AddSingleton<IEventSubConnectionObserver>(sp => sp.GetRequiredService<BotStatusTracker>());

            services.AddSingleton<StreamStatsTracker>();
            services.AddSingleton<IStreamStats>(sp => sp.GetRequiredService<StreamStatsTracker>());
            services.AddSingleton<IStreamStatsWriter>(sp => sp.GetRequiredService<StreamStatsTracker>());

            services.AddSingleton<CommandStatsTracker>();
            services.AddSingleton<ICommandStats>(sp => sp.GetRequiredService<CommandStatsTracker>());
            services.AddSingleton<ICommandStatsWriter>(sp => sp.GetRequiredService<CommandStatsTracker>());

            services.AddSingleton<FollowerStatsTracker>();
            services.AddSingleton<IFollowerStats>(sp => sp.GetRequiredService<FollowerStatsTracker>());
            services.AddSingleton<IFollowerStatsWriter>(sp => sp.GetRequiredService<FollowerStatsTracker>());

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
