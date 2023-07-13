using System.Text.Json.Serialization;
#pragma warning disable CS8618

namespace mapasculturais_service.Entities;


public class CreateTimestamp
{
    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("timezone_type")]
    public int? TimezoneType { get; set; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; set; }
}