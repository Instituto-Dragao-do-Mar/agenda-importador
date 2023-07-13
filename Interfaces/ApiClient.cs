using mapasculturais_service.Configurations;
using Microsoft.Extensions.Options;

namespace mapasculturais_service.Interfaces;

public abstract class ApiClient
{
    protected HttpClient HttpClient { get; private set; }
    private ApiClientConfigurations ApiClientConfigurations { get; set; }
     
    protected ApiClient(IOptions<ApiClientConfigurations> apiClientConfigurations)
    {
        ApiClientConfigurations = apiClientConfigurations.Value;
        HttpClient = new HttpClient {Timeout = TimeSpan.FromSeconds(ApiClientConfigurations.Timeout ?? 30)};
    }

    protected abstract void CheckIfHasSuccess(HttpResponseMessage response);
}