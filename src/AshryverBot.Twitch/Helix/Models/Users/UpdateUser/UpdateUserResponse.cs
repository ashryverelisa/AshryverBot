using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Users.GetUsers;

namespace AshryverBot.Twitch.Helix.Models.Users.UpdateUser;

public record UpdateUserResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<User> Data
);
