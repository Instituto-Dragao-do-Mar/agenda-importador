using System.Text.Json;
using mapasculturais_service.Configurations;
using mapasculturais_service.Entities;
using mapasculturais_service.Interfaces;
using Microsoft.Extensions.Options;

namespace mapasculturais_service.Services;

public class MapasCulturaisService : ApiClient, IMapasCulturaisService
{
    private MapasCulturaisConfiguration _configurations;

    public MapasCulturaisService(IOptions<MapasCulturaisConfiguration> configurations,
        IOptions<ApiClientConfigurations> apiClientConfigurations) : base(apiClientConfigurations)
    {
        _configurations = configurations.Value;
    }

    public async Task<List<Agent>?> GetAprovedEventsFromApi()
    {
        try
        {
            var baseUrl = _configurations.BaseUrl;
            var agentsEndpoint = _configurations.Agents;

            var response = await HttpClient.PostAsync(
                baseUrl + agentsEndpoint + $"?@select=name,spaces,events&id=in({_configurations.AgentsIds})", null);
            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            var responseContent = JsonSerializer.Deserialize<List<Agent>>(stringContent);

            return responseContent;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Event>?> GetHighlightedEventsFromApi()
    {
        try
        {
            var baseUrl = _configurations.BaseUrl;
            var eventsEndpoint = _configurations.Events;

            var response = await HttpClient.PostAsync(baseUrl + eventsEndpoint + "?@seals=13", null);
            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            var responseContent = JsonSerializer.Deserialize<List<Event>>(stringContent);

            return responseContent;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Space>?> GetAprovedSpacesFromApi()
    {
        try
        {
            var baseUrl = _configurations.BaseUrl;
            var spacesEndpoint = _configurations.Spaces;

            var response = await HttpClient.PostAsync(baseUrl + spacesEndpoint + "?@seals=12", null);
            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            var responseContent = JsonSerializer.Deserialize<List<Space>>(stringContent);

            return responseContent;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Event>?> GetEventsFromApi()
    {
        try
        {
            var baseUrl = _configurations.BaseUrl;
            var eventsEndpoint = _configurations.Events;

            var response =
                await HttpClient.PostAsync(baseUrl + eventsEndpoint + "?@select=*&@files=(avatar):url", null);
            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            var responseContent = JsonSerializer.Deserialize<List<Event>>(stringContent);

            return responseContent;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<SpaceType>?> GetSpaceTypesFromApi()
    {
        try
        {
            var baseUrl = _configurations.BaseUrl;
            var spaceTypesEndpoint = _configurations.SpaceTypes;

            var response = await HttpClient.PostAsync(baseUrl + spaceTypesEndpoint, null);
            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            var responseContent = JsonSerializer.Deserialize<List<SpaceType>>(stringContent);

            return responseContent;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Space>?> GetSpacesFromApi()
    {
        try
        {
            var baseUrl = _configurations.BaseUrl;
            var spacesEndpoint = _configurations.Spaces;
            var spacesSelectParameters = _configurations.SpacesSelectParameters;

            var response = await HttpClient.PostAsync(
                baseUrl + spacesEndpoint + "?@select=" + spacesSelectParameters + "&@files=(avatar):url"
                // + "&@limit=1000"
                , null);
            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            var responseContent = JsonSerializer.Deserialize<List<Space>>(stringContent);

            return responseContent;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Occurrence>?> GetOccurrencesFromApi()
    {
        try
        {
            var baseUrl = _configurations.BaseUrl;
            var occurrencesEndpoint = _configurations.Occurences;

            var response = await HttpClient.PostAsync(
                baseUrl + occurrencesEndpoint +
                $"?@from={DateTime.Now:yyyy-MM-dd}&@to={DateTime.Now.AddMonths(2):yyyy-MM-dd}&@files=(avatar):url",
                null);
            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            // stringContent = stringContent.Replace(@"spaceId"":""", @"spaceId"":");
            // stringContent = stringContent.Replace(@""",""startsAt", @",""startsAt");
            var responseContent = JsonSerializer.Deserialize<List<Occurrence>>(stringContent);

            return responseContent;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    protected override void CheckIfHasSuccess(HttpResponseMessage response)
    {
        throw new NotImplementedException();
    }
}