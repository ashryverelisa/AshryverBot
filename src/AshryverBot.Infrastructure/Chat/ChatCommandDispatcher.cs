using System.Collections.Concurrent;
using AshryverBot.Database.Repositories.Interfaces;
using AshryverBot.Infrastructure.Chat.Commands;
using AshryverBot.Infrastructure.Chat.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AshryverBot.Infrastructure.Chat;

public interface IChatCommandDispatcher
{
    Task DispatchAsync(ChatMessage message, CancellationToken cancellationToken);
}

public class ChatCommandDispatcher(
    IServiceScopeFactory scopeFactory,
    TimeProvider timeProvider,
    ILogger<ChatCommandDispatcher> logger) : IChatCommandDispatcher
{
    private readonly ConcurrentDictionary<string, DateTimeOffset> _cooldowns
        = new(StringComparer.OrdinalIgnoreCase);

    public async Task DispatchAsync(ChatMessage message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        var text = message.Text;
        if (string.IsNullOrWhiteSpace(text) || text[0] != '!') return;

        var trimmed = text.AsSpan(1).TrimStart();
        if (trimmed.IsEmpty) return;

        var firstSpace = trimmed.IndexOf(' ');
        string name;
        string[] arguments;
        if (firstSpace < 0)
        {
            name = trimmed.ToString();
            arguments = [];
        }
        else
        {
            name = trimmed[..firstSpace].ToString();
            arguments = trimmed[(firstSpace + 1)..]
                .ToString()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }

        await using var scope = scopeFactory.CreateAsyncScope();

        var staticMatch = scope.ServiceProvider.GetServices<IChatCommand>()
            .FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));

        if (staticMatch is not null)
        {
            try
            {
                await staticMatch.ExecuteAsync(new ChatCommandContext(message, arguments), cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Chat command '!{Name}' failed.", name);
            }
            return;
        }

        var repository = scope.ServiceProvider.GetRequiredService<ICommandRepository>();
        var entity = await repository.GetByNameAsync(name, cancellationToken);
        if (entity is null || !entity.IsEnabled) return;

        if (!TryAcquireCooldown(entity.Name, entity.CooldownSeconds))
        {
            logger.LogDebug("Chat command '!{Name}' is on cooldown — skipped.", entity.Name);
            return;
        }

        var responder = scope.ServiceProvider.GetRequiredService<IChatResponder>();
        try
        {
            await responder.SendAsync(
                message.BroadcasterUserId,
                entity.Response,
                cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Chat command '!{Name}' (db) failed.", name);
        }
    }

    private bool TryAcquireCooldown(string commandName, int cooldownSeconds)
    {
        if (cooldownSeconds <= 0) return true;

        var now = timeProvider.GetUtcNow();
        var window = TimeSpan.FromSeconds(cooldownSeconds);

        while (true)
        {
            if (_cooldowns.TryGetValue(commandName, out var last))
            {
                if (now - last < window) return false;

                if (_cooldowns.TryUpdate(commandName, now, last)) return true;
            }
            else if (_cooldowns.TryAdd(commandName, now))
            {
                return true;
            }
        }
    }
}
