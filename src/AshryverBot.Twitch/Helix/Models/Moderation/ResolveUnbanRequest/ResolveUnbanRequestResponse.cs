using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Moderation.GetUnbanRequests;

namespace AshryverBot.Twitch.Helix.Models.Moderation.ResolveUnbanRequest;

public record ResolveUnbanRequestResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<UnbanRequest> Data
);
