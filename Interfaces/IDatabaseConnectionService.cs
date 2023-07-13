using mapasculturais_service.Entities;

namespace mapasculturais_service.Interfaces;

public interface IDatabaseConnectionService
{
    // Task SaveSpaceTypeListToDatabase(List<SpaceType> entities);
    Task SaveSpaceListToDatabase(List<Space> entities, Agent? agent = null);
    Task SaveEventListToDatabase(List<Event> entities, Agent? agent = null);
    Task SaveOccurrenceListToDatabase(List<Occurrence> entities);
    // Task UpdateSpacesSetVerifiedByAgent(List<int> spacesIdsLists);
    // Task UpdateEventsSetVerifiedByAgent(List<int> eventsIdsLists);
}