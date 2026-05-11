using System.Text.Json.Serialization;

namespace AshryverBot.Twitch.Helix.Models.Moderation.UpdateAutoModSettings;

public record UpdateAutoModSettingsRequest
{
    [JsonPropertyName("overall_level")]
    public int? OverallLevel { get; init; }

    [JsonPropertyName("disability")]
    public int? Disability { get; init; }

    [JsonPropertyName("aggression")]
    public int? Aggression { get; init; }

    [JsonPropertyName("sexuality_sex_or_gender")]
    public int? SexualitySexOrGender { get; init; }

    [JsonPropertyName("misogyny")]
    public int? Misogyny { get; init; }

    [JsonPropertyName("bullying")]
    public int? Bullying { get; init; }

    [JsonPropertyName("swearing")]
    public int? Swearing { get; init; }

    [JsonPropertyName("race_ethnicity_or_religion")]
    public int? RaceEthnicityOrReligion { get; init; }

    [JsonPropertyName("sex_based_terms")]
    public int? SexBasedTerms { get; init; }
}
