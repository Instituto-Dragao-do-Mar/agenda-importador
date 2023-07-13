using System.Text.Json.Serialization;

namespace mapasculturais_service.Entities;
#pragma warning disable CS8618

public class Day
{
    [JsonPropertyName("1")]
    public string? _1 { get; set; }
    [JsonPropertyName("2")]
    public string? _2 { get; set; }
    [JsonPropertyName("3")]
    public string? _3 { get; set; }
    [JsonPropertyName("4")]
    public string? _4 { get; set; }
    [JsonPropertyName("5")]
    public string? _5 { get; set; }
    [JsonPropertyName("6")]
    public string? _6 { get; set; }
    [JsonPropertyName("7")]
    public string? _7 { get; set; }
}