using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Games.Common;

namespace AshryverBot.Twitch.Helix.Models.Games.GetGames;

public record GetGamesResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<Game> Data
);
