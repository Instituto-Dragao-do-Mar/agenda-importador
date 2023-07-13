using mapasculturais_service.Entities;

namespace mapasculturais_service.Interfaces;

public interface IMapasCulturaisService
{
    Task<List<Agent>?> GetAprovedEventsFromApi();
    Task<List<Space>?> GetAprovedSpacesFromApi();
    Task<List<Event>?> GetHighlightedEventsFromApi();
    Task<List<Event>?> GetEventsFromApi();
    Task<List<SpaceType>?> GetSpaceTypesFromApi();
    Task<List<Space>?> GetSpacesFromApi();
    Task<List<Occurrence>?> GetOccurrencesFromApi();
}