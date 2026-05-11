using System.Text.Json.Serialization;
using AshryverBot.Twitch.Helix.Models.Common;

namespace AshryverBot.Twitch.Helix.Models.Bits.GetExtensionTransactions;

public record GetExtensionTransactionsResponse(
    [property: JsonPropertyName("data")] IReadOnlyCollection<ExtensionTransaction> Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
