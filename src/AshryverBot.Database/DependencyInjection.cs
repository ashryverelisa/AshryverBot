using AshryverBot.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AshryverBot.Database;

public static class DependencyInjection
{
    private const string ConnectionStringName = "AshryverBot";

    public static IServiceCollection AddAshryverBotDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName)
            ?? throw new InvalidOperationException(
                $"Connection string 'ConnectionStrings:{ConnectionStringName}' is not configured.");

        services.AddDbContextFactory<AshryverBotDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsAssembly(typeof(AshryverBotDbContext).Assembly.GetName().Name)));

        services.AddScoped<ITwitchTokenRepository, TwitchTokenRepository>();

        return services;
    }
}
