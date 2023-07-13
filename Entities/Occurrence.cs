using System.Text.Json.Serialization;

namespace mapasculturais_service.Entities;

public class Occurrence : BaseEntity
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("occurrence_id")]
    public int? OccurrenceId { get; set; }

    [JsonPropertyName("event_id")]
    public int? EventId { get; set; }

    [JsonPropertyName("starts_on")]
    public string? StartsOn { get; set; }

    [JsonPropertyName("starts_at")]
    public string? StartsAt { get; set; }

    [JsonPropertyName("ends_on")]
    public object EndsOn { get; set; }

    [JsonPropertyName("ends_at")]
    public string EndsAt { get; set; }

    [JsonPropertyName("rule")]
    public Rule? Rule { get; set; }

    [JsonPropertyName("space")]
    public Space? Space { get; set; }

    [JsonPropertyName("attendance")]
    public object Attendance { get; set; }

    [JsonPropertyName("_reccurrence_string")]
    public string ReccurrenceString { get; set; }
    
    [JsonPropertyName("@files:avatar")]
    public FilesAvatar? FilesAvatar { get; set; }
}

