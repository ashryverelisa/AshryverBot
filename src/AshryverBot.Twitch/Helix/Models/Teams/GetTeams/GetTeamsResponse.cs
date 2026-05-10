using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Teams.Common;

namespace AshryverBot.Twitch.Helix.Models.Teams.GetTeams;

public record GetTeamsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<Team> Data
);
