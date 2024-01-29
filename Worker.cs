using mapasculturais_service.Entities;
using mapasculturais_service.Interfaces;
using static System.Threading.Tasks.Task;

namespace mapasculturais_service;

public class Worker : BackgroundService
{
    private readonly IMapasCulturaisService _mapasCulturaisService;
    private readonly IDatabaseConnectionService _databaseConnectionService;
    private readonly ILogger<Worker> _logger;

    private IConfiguration Configuration { get; }

    public Worker(IMapasCulturaisService mapasCulturaisService, ILogger<Worker> logger, IConfiguration configuration,
        IDatabaseConnectionService databaseConnectionService)
    {
        _mapasCulturaisService = mapasCulturaisService;
        _logger = logger;
        Configuration = configuration;
        _databaseConnectionService = databaseConnectionService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var agents = await _mapasCulturaisService.GetAprovedEventsFromApi();
            var eventsIdsLists = (agents ?? new List<Agent>()).SelectMany(a => a.EventsIds).ToList();
            
            var aprovedSpaces = await _mapasCulturaisService.GetAprovedSpacesFromApi();
            var spacesIdsLists = (aprovedSpaces ?? new List<Space>()).Select(s => s.Id).ToList();

            var a = string.Join<int>(",", spacesIdsLists);
            var s = string.Join<int>(",", eventsIdsLists);
            
            var highlightedEvents = await _mapasCulturaisService.GetHighlightedEventsFromApi();
            var highlightedEventsIdsLists = (highlightedEvents ?? new List<Event>()).Select(e => e.Id).ToList();

            var agent = new Agent
            {
                SpacesIds = spacesIdsLists,
                EventsIds = eventsIdsLists,
                HighlightedEventsIds = highlightedEventsIdsLists
            };

            // await _databaseConnectionService.UpdateSpacesSetVerifiedByAgent(spacesIdsLists);
            // await _databaseConnectionService.UpdateEventsSetVerifiedByAgent(eventsIdsLists);


            Console.Write("");

            // var spaceTypes = await _mapasCulturaisService.GetSpaceTypesFromApi();
            // if (spaceTypes != null)
            //     await _databaseConnectionService.SaveSpaceTypeListToDatabase(spaceTypes)
            //         .WaitAsync(CancellationToken.None);
            // await Delay(5000, stoppingToken);
            
            
            // ------------------------------------------------------------------------------------------------------
            var spaces = await _mapasCulturaisService.GetSpacesFromApi();
            if (spaces != null)
                await _databaseConnectionService.SaveSpaceListToDatabase(spaces, agent)
                    .WaitAsync(CancellationToken.None);
            await Delay(5000, stoppingToken);
            
            var events = await _mapasCulturaisService.GetEventsFromApi();
            if (events != null)
                await _databaseConnectionService.SaveEventListToDatabase(events, agent)
                    .WaitAsync(CancellationToken.None);
            await Delay(5000, stoppingToken);

            var occurrences = await _mapasCulturaisService.GetOccurrencesFromApi();
            if (occurrences != null)
                await _databaseConnectionService.SaveOccurrenceListToDatabase(occurrences)
                    .WaitAsync(CancellationToken.None);

            _logger.LogInformation("Última execução: {Time}", DateTimeOffset.Now);
            var delayTime = int.Parse(Configuration.GetSection("DelayTime").Value);
            await Delay(delayTime, stoppingToken);
        }
    }
}