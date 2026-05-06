using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Users.GetUsers;

public record GetUsersResponse
(
    [property: JsonPropertyName("data")] IReadOnlyCollection<User> Users
);