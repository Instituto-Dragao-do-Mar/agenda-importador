using System.Text.Json.Serialization;
using Newtonsoft.Json;
#pragma warning disable CS8618

namespace mapasculturais_service.Entities;

public class Space : BaseEntity
{

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("location")]
    public Location? Location { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("public")]
    public bool Public { get; set; }

    [JsonPropertyName("shortDescription")]
    public string? ShortDescription { get; set; }

    [JsonPropertyName("longDescription")]
    public string? LongDescription { get; set; }

    [JsonPropertyName("createTimestamp")]
    public CreateTimestamp? CreateTimestamp { get; set; }

    [JsonPropertyName("status")]
    public int? Status { get; set; }

    [JsonPropertyName("updateTimestamp")]
    public UpdateTimestamp? UpdateTimestamp { get; set; }

    [JsonPropertyName("parent")]
    public int? Parent { get; set; }

    [JsonPropertyName("owner")]
    public int? Owner { get; set; }

    [JsonPropertyName("subsite")]
    public object? Subsite { get; set; }

    [JsonPropertyName("capacidade")]
    public string? Capacidade { get; set; }

    [JsonPropertyName("horario")]
    public string? Horario { get; set; }

    [JsonPropertyName("endereco")]
    public string? Endereco { get; set; }

    [JsonPropertyName("telefonePublico")]
    public string? TelefonePublico { get; set; }

    [JsonPropertyName("eventOccurrences")]
    public List<int>? EventOccurrences { get; set; }

    [JsonPropertyName("children")]
    public List<object>? Children { get; set; }

    [JsonPropertyName("relatedOpportunities")]
    public List<object>? RelatedOpportunities { get; set; }

    [JsonPropertyName("terms")]
    public Terms? Terms { get; set; }

    [JsonPropertyName("type")]
    public SpaceType? SpaceType { get; set; }

    [JsonPropertyName("ownerId")]
    public object? OwnerId { get; set; }

    [JsonPropertyName("subsiteId")]
    public object? SubsiteId { get; set; }

    [JsonPropertyName("emailPublico")]
    public object? EmailPublico { get; set; }

    [JsonPropertyName("emailPrivado")]
    public object? EmailPrivado { get; set; }

    [JsonPropertyName("telefone1")]
    public object? Telefone1 { get; set; }

    [JsonPropertyName("telefone2")]
    public object? Telefone2 { get; set; }

    [JsonPropertyName("acessibilidade")]
    public string? Acessibilidade { get; set; }

    [JsonPropertyName("acessibilidade_fisica")]
    public string? AcessibilidadeFisica { get; set; }

    [JsonPropertyName("En_CEP")]
    public string? EnCep { get; set; }

    [JsonPropertyName("En_Nome_Logradouro")]
    public string? EnNomeLogradouro { get; set; }

    [JsonPropertyName("En_Num")]
    public string? EnNum { get; set; }

    [JsonPropertyName("En_Complemento")]
    public string? EnComplemento { get; set; }

    [JsonPropertyName("En_Bairro")]
    public string? EnBairro { get; set; }

    [JsonPropertyName("En_Municipio")]
    public string? EnMunicipio { get; set; }

    [JsonPropertyName("En_Estado")]
    public string? EnEstado { get; set; }

    [JsonPropertyName("criterios")]
    public object? Criterios { get; set; }

    [JsonPropertyName("site")]
    public string? Site { get; set; }

    [JsonPropertyName("facebook")]
    public string? Facebook { get; set; }

    [JsonPropertyName("twitter")]
    public string? Twitter { get; set; }

    [JsonPropertyName("googleplus")]
    public object? Googleplus { get; set; }

    [JsonPropertyName("instagram")]
    public string? Instagram { get; set; }

    [JsonPropertyName("geoPais")]
    public object? GeoPais { get; set; }

    [JsonPropertyName("geoPais_cod")]
    public object? GeoPaisCod { get; set; }

    [JsonPropertyName("geoRegiao")]
    public object? GeoRegiao { get; set; }

    [JsonPropertyName("geoRegiao_cod")]
    public object? GeoRegiaoCod { get; set; }

    [JsonPropertyName("geoEstado")]
    public object? GeoEstado { get; set; }

    [JsonPropertyName("geoEstado_cod")]
    public object? GeoEstadoCod { get; set; }

    [JsonPropertyName("geoMesorregiao")]
    public object? GeoMesorregiao { get; set; }

    [JsonPropertyName("geoMesorregiao_cod")]
    public object? GeoMesorregiaoCod { get; set; }

    [JsonPropertyName("geoMicrorregiao")]
    public object? GeoMicrorregiao { get; set; }

    [JsonPropertyName("geoMicrorregiao_cod")]
    public object? GeoMicrorregiaoCod { get; set; }

    [JsonPropertyName("geoMunicipio")]
    public object? GeoMunicipio { get; set; }

    [JsonPropertyName("geoMunicipio_cod")]
    public object? GeoMunicipioCod { get; set; }

    [JsonPropertyName("geoZona")]
    public object? GeoZona { get; set; }

    [JsonPropertyName("geoZona_cod")]
    public object? GeoZonaCod { get; set; }

    [JsonPropertyName("geoSubprefeitura")]
    public object? GeoSubprefeitura { get; set; }

    [JsonPropertyName("geoSubprefeitura_cod")]
    public object? GeoSubprefeituraCod { get; set; }

    [JsonPropertyName("geoDistrito")]
    public object? GeoDistrito { get; set; }

    [JsonPropertyName("geoDistrito_cod")]
    public object? GeoDistritoCod { get; set; }

    [JsonPropertyName("geoSetor_censitario")]
    public object? GeoSetorCensitario { get; set; }

    [JsonPropertyName("geoSetor_censitario_cod")]
    public object? GeoSetorCensitarioCod { get; set; }

    [JsonPropertyName("opportunityTabName")]
    public object? OpportunityTabName { get; set; }

    [JsonPropertyName("useOpportunityTab")]
    public object? UseOpportunityTab { get; set; }

    [JsonPropertyName("sentNotification")]
    public object? SentNotification { get; set; }
    
    [JsonPropertyName("@files:avatar")]
    public FilesAvatar? FilesAvatar { get; set; }
}




