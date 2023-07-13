using System.Text.Json.Serialization;
#pragma warning disable CS8618

namespace mapasculturais_service.Entities;


public class Terms
{
    [JsonPropertyName("tag")]
    public List<string>? Tag { get; set; }
    [JsonPropertyName("linguagem")]
    public List<string>? Linguagem { get; set; }

    [JsonPropertyName("area")]
    public List<string>? Area { get; set; }
}