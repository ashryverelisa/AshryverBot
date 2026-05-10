using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;
using AshryverBot.Twitch.Helix.Models.Search.Common;

namespace AshryverBot.Twitch.Helix.Models.Search.SearchCategories;

public record SearchCategoriesResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<SearchedCategory> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
