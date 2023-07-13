using System.Text.Json.Serialization;
using Newtonsoft.Json;
#pragma warning disable CS8618

namespace mapasculturais_service.Entities;

public class Event : BaseEntity
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("shortDescription")]
        public string? ShortDescription { get; set; }

        [JsonPropertyName("longDescription")]
        public string? LongDescription { get; set; }

        [JsonPropertyName("rules")]
        public object Rules { get; set; }

        [JsonPropertyName("createTimestamp")]
        public CreateTimestamp? CreateTimestamp { get; set; }

        [JsonPropertyName("status")]
        public int? Status { get; set; }

        [JsonPropertyName("updateTimestamp")]
        public UpdateTimestamp? UpdateTimestamp { get; set; }

        [JsonPropertyName("owner")]
        public int? Owner { get; set; }

        [JsonPropertyName("project")]
        public object Project { get; set; }

        [JsonPropertyName("subsite")]
        public object Subsite { get; set; }

        [JsonPropertyName("subTitle")]
        public string? SubTitle { get; set; }

        [JsonPropertyName("classificacaoEtaria")]
        public string? ClassificacaoEtaria { get; set; }

        [JsonPropertyName("site")]
        public string? Site { get; set; }

        [JsonPropertyName("telefonePublico")]
        public string? TelefonePublico { get; set; }

        [JsonPropertyName("occurrences")]
        public List<object> Occurrences { get; set; }

        [JsonPropertyName("relatedOpportunities")]
        public List<object> RelatedOpportunities { get; set; }

        [JsonPropertyName("terms")]
        public Terms? Terms { get; set; }

        [JsonPropertyName("type")]
        public SpaceType SpaceType { get; set; }

        [JsonPropertyName("subsiteId")]
        public object SubsiteId { get; set; }

        [JsonPropertyName("registrationInfo")]
        public object RegistrationInfo { get; set; }

        [JsonPropertyName("preco")]
        public object Preco { get; set; }

        [JsonPropertyName("traducaoLibras")]
        public object TraducaoLibras { get; set; }

        [JsonPropertyName("descricaoSonora")]
        public object DescricaoSonora { get; set; }

        [JsonPropertyName("facebook")]
        public object Facebook { get; set; }

        [JsonPropertyName("twitter")]
        public object Twitter { get; set; }

        [JsonPropertyName("googleplus")]
        public object Googleplus { get; set; }

        [JsonPropertyName("instagram")]
        public object Instagram { get; set; }

        [JsonPropertyName("event_attendance")]
        public object EventAttendance { get; set; }

        [JsonPropertyName("opportunityTabName")]
        public object OpportunityTabName { get; set; }

        [JsonPropertyName("useOpportunityTab")]
        public object UseOpportunityTab { get; set; }
        
        [JsonPropertyName("@files:avatar")]
        public FilesAvatar? FilesAvatar { get; set; }
    }

