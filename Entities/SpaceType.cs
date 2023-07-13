using System.Text.Json.Serialization;
using Newtonsoft.Json;
#pragma warning disable CS8618

namespace mapasculturais_service.Entities;


public class SpaceType : BaseEntity
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}