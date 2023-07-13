using System.Text.Json.Serialization;

namespace mapasculturais_service.Entities;

public class Location
{
    [JsonPropertyName("latitude")]
    public string Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public string Longitude { get; set; }
}