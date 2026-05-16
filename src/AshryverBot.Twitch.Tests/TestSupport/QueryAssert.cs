namespace AshryverBot.Twitch.Tests.TestSupport;

internal static class QueryAssert
{
    extension(IReadOnlyList<KeyValuePair<string, string>>? list)
    {
        public bool Has(string key, string value)
            => list is not null && list.Any(kv => kv.Key == key && kv.Value == value);

        public bool HasNoKey(string key)
            => list is null || list.All(kv => kv.Key != key);

        public int CountKey(string key)
            => list?.Count(kv => kv.Key == key) ?? 0;

        public int TotalCount()
            => list?.Count ?? 0;
    }
}
