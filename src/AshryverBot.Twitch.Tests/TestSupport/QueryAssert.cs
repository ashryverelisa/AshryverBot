namespace AshryverBot.Twitch.Tests.TestSupport;

internal static class QueryAssert
{
    public static bool Has(this IReadOnlyList<KeyValuePair<string, string>>? list, string key, string value)
        => list is not null && list.Any(kv => kv.Key == key && kv.Value == value);

    public static bool HasNoKey(this IReadOnlyList<KeyValuePair<string, string>>? list, string key)
        => list is null || list.All(kv => kv.Key != key);

    public static int CountKey(this IReadOnlyList<KeyValuePair<string, string>>? list, string key)
        => list?.Count(kv => kv.Key == key) ?? 0;

    public static int TotalCount(this IReadOnlyList<KeyValuePair<string, string>>? list)
        => list?.Count ?? 0;
}
