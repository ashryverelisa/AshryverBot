using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AshryverBot.Infrastructure.Chat;

public interface IChatCommandDispatcher
{
    Task DispatchAsync(ChatMessage message, CancellationToken cancellationToken);
}

public class ChatCommandDispatcher(
    IServiceScopeFactory scopeFactory,
    ILogger<ChatCommandDispatcher> logger) : IChatCommandDispatcher
{
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
        var commands = scope.ServiceProvider.GetServices<IChatCommand>();
        var match = commands.FirstOrDefault(c =>
            string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));

        if (match is null) return;

        try
        {
            await match.ExecuteAsync(new ChatCommandContext(message, arguments), cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Chat command '!{Name}' failed.", name);
        }
    }
}
