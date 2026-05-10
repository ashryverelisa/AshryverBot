using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Channels.GetChannelInformation;

public record GetChannelInformationResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ChannelInformation> Data
);
