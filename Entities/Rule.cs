using System.Text.Json.Serialization;

namespace mapasculturais_service.Entities;
#pragma warning disable CS8618

public class Rule
{
    // [JsonPropertyName("spaceId")]
    // public int SpaceId { get; set; }

    [JsonPropertyName("startsAt")]
    public string StartsAt { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }

    [JsonPropertyName("endsAt")]
    public string EndsAt { get; set; }

    [JsonPropertyName("frequency")]
    public string Frequency { get; set; }

    [JsonPropertyName("startsOn")]
    public string StartsOn { get; set; }

    [JsonPropertyName("until")]
    public string Until { get; set; }
    
    // [JsonPropertyName("day")]
    // public Day? Day { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("price")]
    public string Price { get; set; }
}