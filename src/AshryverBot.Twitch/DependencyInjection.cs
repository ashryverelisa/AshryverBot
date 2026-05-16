using AshryverBot.Twitch.Clients;
using AshryverBot.Twitch.Clients.Interfaces;
using AshryverBot.Twitch.Configuration;
using AshryverBot.Twitch.EventSub.WebSocket;
using AshryverBot.Twitch.Helix.Apis;
using AshryverBot.Twitch.Helix.Apis.Interfaces;
using AshryverBot.Twitch.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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

        services.TryAddSingleton(TimeProvider.System);
        services.AddTransient<TwitchRateLimitHandler>();

        services.AddHttpClient<ITwitchOAuthClient, TwitchOAuthClient>(client =>
        {
            client.BaseAddress = new Uri(Constants.BaseAuth.TrimEnd('/') + "/");
        });

        services.AddHttpClient<ITwitchClient, TwitchClient>(client =>
        {
            client.BaseAddress = new Uri(Constants.BaseHelix.TrimEnd('/') + "/");
        })
        .AddHttpMessageHandler<TwitchRateLimitHandler>();

        services.AddScoped<IAnalyticsApi, AnalyticsApi>();
        services.AddScoped<IBitsApi, BitsApi>();
        services.AddScoped<IChannelPointsApi, ChannelPointsApi>();
        services.AddScoped<IChannelsApi, ChannelsApi>();
        services.AddScoped<ICharityApi, CharityApi>();
        services.AddScoped<IChatApi, ChatApi>();
        services.AddScoped<IClipsApi, ClipsApi>();
        services.AddScoped<IConduitsApi, ConduitsApi>();
        services.AddScoped<IContentClassificationLabelsApi, ContentClassificationLabelsApi>();
        services.AddScoped<IEntitlementsApi, EntitlementsApi>();
        services.AddScoped<IEventSubApi, EventSubApi>();
        services.AddScoped<IExtensionsApi, ExtensionsApi>();
        services.AddScoped<IGamesApi, GamesApi>();
        services.AddScoped<IGoalsApi, GoalsApi>();
        services.AddScoped<IGuestStarApi, GuestStarApi>();
        services.AddScoped<IHypeTrainApi, HypeTrainApi>();
        services.AddScoped<IModerationApi, ModerationApi>();
        services.AddScoped<IPollsApi, PollsApi>();
        services.AddScoped<IPredictionsApi, PredictionsApi>();
        services.AddScoped<IRaidsApi, RaidsApi>();
        services.AddScoped<IScheduleApi, ScheduleApi>();
        services.AddScoped<ISearchApi, SearchApi>();
        services.AddScoped<IStreamsApi, StreamsApi>();
        services.AddScoped<ISubscriptionsApi, SubscriptionsApi>();
        services.AddScoped<ITeamsApi, TeamsApi>();
        services.AddScoped<IUsersApi, UsersApi>();
        services.AddScoped<IVideosApi, VideosApi>();
        services.AddScoped<IWhispersApi, WhispersApi>();

        return services;
    }

    public static IServiceCollection AddTwitchEventSubWebSocket(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<EventSubWebSocketOptions>()
            .Bind(configuration.GetSection(EventSubWebSocketOptions.SectionName));

        services.AddSingleton<EventSubWebSocketClient>();

        return services;
    }
}
