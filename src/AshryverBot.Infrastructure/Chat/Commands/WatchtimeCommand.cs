using System.Text;
using AshryverBot.Database.Repositories;

namespace AshryverBot.Infrastructure.Chat.Commands;

public class WatchtimeCommand(
    IWatchtimeRepository repository,
    IChatResponder responder) : IChatCommand
{
    public string Name => "watchtime";

    public async Task ExecuteAsync(ChatCommandContext context, CancellationToken cancellationToken)
    {
        var msg = context.Message;
        var entity = await repository.GetByUserIdAsync(msg.ChatterUserId, cancellationToken);
        var displayName = string.IsNullOrWhiteSpace(msg.ChatterDisplayName)
            ? msg.ChatterUserLogin
            : msg.ChatterDisplayName;

        string reply;
        if (entity is null || entity.TotalSeconds <= 0)
        {
            reply = $"@{displayName} du hast den Stream noch nicht geschaut.";
        }
        else
        {
            var formatted = FormatDuration(TimeSpan.FromSeconds(entity.TotalSeconds));
            reply = $"@{displayName} hast {formatted} geschaut";
        }

        await responder.SendAsync(msg.BroadcasterUserId, reply, cancellationToken);
    }

    internal static string FormatDuration(TimeSpan duration)
    {
        if (duration < TimeSpan.FromMinutes(1))
            return "weniger als 1m";

        var days = (int)duration.TotalDays;
        var hours = duration.Hours;
        var minutes = duration.Minutes;

        var sb = new StringBuilder();
        if (days > 0)
        {
            sb.Append(days).Append('d');
            sb.Append(' ').Append(hours).Append('h');
            sb.Append(' ').Append(minutes).Append('m');
        }
        else if (hours > 0)
        {
            sb.Append(hours).Append('h');
            sb.Append(' ').Append(minutes).Append('m');
        }
        else
        {
            sb.Append(minutes).Append('m');
        }

        return sb.ToString();
    }
}
