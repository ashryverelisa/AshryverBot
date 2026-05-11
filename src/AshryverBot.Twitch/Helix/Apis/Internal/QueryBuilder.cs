using System.Globalization;

namespace AshryverBot.Twitch.Helix.Apis.Internal;

internal static class QueryBuilder
{
    extension(List<KeyValuePair<string, string>> list)
    {
        public void AddIfNotNull(string key, string? value)
        {
            if (value is not null) list.Add(new KeyValuePair<string, string>(key, value));
        }

        public void AddIfNotNull(string key, int? value)
        {
            if (value.HasValue) list.Add(new KeyValuePair<string, string>(key, value.Value.ToString(CultureInfo.InvariantCulture)));
        }

        public void AddIfNotNull(string key, long? value)
        {
            if (value.HasValue) list.Add(new KeyValuePair<string, string>(key, value.Value.ToString(CultureInfo.InvariantCulture)));
        }

        public void AddIfNotNull(string key, bool? value)
        {
            if (value.HasValue) list.Add(new KeyValuePair<string, string>(key, value.Value ? "true" : "false"));
        }

        public void AddIfNotNull(string key, DateTimeOffset? value)
        {
            if (value.HasValue) list.Add(new KeyValuePair<string, string>(key, value.Value.ToString("O", CultureInfo.InvariantCulture)));
        }

        public void AddMany(string key, IEnumerable<string>? values)
        {
            if (values is null) return;
            list.AddRange(values.Select(v => new KeyValuePair<string, string>(key, v)));
        }
    }
}