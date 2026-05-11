using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Teams.GetChannelTeams;

public record GetChannelTeamsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ChannelTeam> Data
);
