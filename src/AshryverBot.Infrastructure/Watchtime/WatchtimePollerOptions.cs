namespace AshryverBot.Infrastructure.Watchtime;

public class WatchtimePollerOptions
{
    public const string SectionName = "Watchtime";

    public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(1);
}
