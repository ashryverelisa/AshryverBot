using AshryverBot.Twitch.Helix.Apis.Internal;
using Xunit;

namespace AshryverBot.Twitch.Tests.Helix.Apis.Internal;

public class QueryBuilderTests
{
    [Fact]
    public void AddIfNotNull_string_adds_when_not_null()
    {
        var list = new List<KeyValuePair<string, string>>();
        list.AddIfNotNull("k", "v");
        Assert.Single(list);
        Assert.Equal(new KeyValuePair<string, string>("k", "v"), list[0]);
    }

    [Fact]
    public void AddIfNotNull_string_skips_when_null()
    {
        var list = new List<KeyValuePair<string, string>>();
        list.AddIfNotNull("k", (string?)null);
        Assert.Empty(list);
    }

    [Fact]
    public void AddIfNotNull_string_keeps_empty_string()
    {
        var list = new List<KeyValuePair<string, string>>();
        list.AddIfNotNull("k", "");
        Assert.Single(list);
        Assert.Equal("", list[0].Value);
    }

    [Fact]
    public void AddIfNotNull_int_uses_invariant_culture()
    {
        var list = new List<KeyValuePair<string, string>>();
        list.AddIfNotNull("k", 42);
        Assert.Equal("42", list[0].Value);
    }

    [Fact]
    public void AddIfNotNull_int_skips_when_null()
    {
        var list = new List<KeyValuePair<string, string>>();
        list.AddIfNotNull("k", (int?)null);
        Assert.Empty(list);
    }

    [Fact]
    public void AddIfNotNull_long_uses_invariant_culture()
    {
        var list = new List<KeyValuePair<string, string>>();
        list.AddIfNotNull("k", 1234567890123L);
        Assert.Equal("1234567890123", list[0].Value);
    }

    [Theory]
    [InlineData(true, "true")]
    [InlineData(false, "false")]
    public void AddIfNotNull_bool_emits_lowercase(bool value, string expected)
    {
        var list = new List<KeyValuePair<string, string>>();
        list.AddIfNotNull("k", value);
        Assert.Equal(expected, list[0].Value);
    }

    [Fact]
    public void AddIfNotNull_bool_skips_when_null()
    {
        var list = new List<KeyValuePair<string, string>>();
        list.AddIfNotNull("k", (bool?)null);
        Assert.Empty(list);
    }

    [Fact]
    public void AddIfNotNull_dateTimeOffset_iso8601_round_trippable()
    {
        var list = new List<KeyValuePair<string, string>>();
        var dt = new DateTimeOffset(2026, 5, 11, 14, 30, 0, TimeSpan.FromHours(2));
        list.AddIfNotNull("k", dt);
        var parsed = DateTimeOffset.Parse(list[0].Value);
        Assert.Equal(dt, parsed);
    }

    [Fact]
    public void AddMany_adds_one_entry_per_value()
    {
        var list = new List<KeyValuePair<string, string>>();
        list.AddMany("id", ["a", "b", "c"]);
        Assert.Equal(3, list.Count);
        Assert.All(list, kv => Assert.Equal("id", kv.Key));
        Assert.Equal(["a", "b", "c"], list.Select(kv => kv.Value));
    }

    [Fact]
    public void AddMany_skips_when_null()
    {
        var list = new List<KeyValuePair<string, string>>();
        list.AddMany("id", null);
        Assert.Empty(list);
    }

    [Fact]
    public void AddMany_empty_collection_adds_nothing()
    {
        var list = new List<KeyValuePair<string, string>>();
        list.AddMany("id", []);
        Assert.Empty(list);
    }
}
