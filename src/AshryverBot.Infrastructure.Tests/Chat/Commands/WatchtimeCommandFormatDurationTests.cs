using AshryverBot.Infrastructure.Chat.Commands;
using Xunit;

namespace AshryverBot.Infrastructure.Tests.Chat.Commands;

public class WatchtimeCommandFormatDurationTests
{
    [Theory]
    [InlineData(0, "weniger als 1m")]
    [InlineData(59, "weniger als 1m")]
    public void Sub_minute_durations_render_as_less_than_one_minute(int seconds, string expected)
    {
        var actual = WatchtimeCommand.FormatDuration(TimeSpan.FromSeconds(seconds));

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(60, "1m")]
    [InlineData(59 * 60, "59m")]
    public void Sub_hour_durations_show_minutes_only(int seconds, string expected)
    {
        var actual = WatchtimeCommand.FormatDuration(TimeSpan.FromSeconds(seconds));

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(1, 0, "1h 0m")]
    [InlineData(2, 14, "2h 14m")]
    [InlineData(23, 59, "23h 59m")]
    public void Sub_day_durations_show_hours_and_minutes(int hours, int minutes, string expected)
    {
        var duration = TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes);

        var actual = WatchtimeCommand.FormatDuration(duration);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(1, 0, 0, "1d 0h 0m")]
    [InlineData(3, 4, 5, "3d 4h 5m")]
    public void Multi_day_durations_show_days_hours_minutes(int days, int hours, int minutes, string expected)
    {
        var duration = TimeSpan.FromDays(days) + TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes);

        var actual = WatchtimeCommand.FormatDuration(duration);

        Assert.Equal(expected, actual);
    }
}
