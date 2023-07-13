using System.Text.Json.Serialization;

namespace mapasculturais_service.Entities;

public class Agent
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("spaces")] public List<int> SpacesIds { get; set; }

    [JsonPropertyName("events")] public List<int> EventsIds { get; set; }
    
    public List<int> HighlightedSpacesIds { get; set; }
    
    public List<int> HighlightedEventsIds { get; set; }
}