using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;
using AshryverBot.Twitch.Helix.Models.Games.Common;

namespace AshryverBot.Twitch.Helix.Models.Games.GetTopGames;

public record GetTopGamesResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<Game> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
