using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Channels.GetChannelEditors;

public record GetChannelEditorsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ChannelEditor> Data
);
