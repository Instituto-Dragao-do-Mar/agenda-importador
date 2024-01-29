using Domain.Utils;
using mapasculturais_service.Configurations;
using mapasculturais_service.Entities;
using mapasculturais_service.Interfaces;
using mapasculturais_service.Services;

namespace mapasculturais_service;

public class Program
{
    public static async Task Main(string[] args)
    {
       var builder =  CreateHostBuilder(args)
           .UseSystemd() //Run in Linux
           // .UseWindowsService(options =>
           // {
           //     options.ServiceName = "Mapas Culturais Import Service";
           // }) //Run in Windows
           .UseEnvironment(EnvironmentUtils.GetEnvironment())
           .Build();
       await builder.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                var envType = EnvironmentUtils.GetEnvironment();
                IConfiguration configuration = hostContext.Configuration;
                services.Configure<ApiClientConfigurations>(configuration.GetSection(nameof(ApiClientConfigurations)));

                services.AddSingleton<IMapasCulturaisService, MapasCulturaisService>();
                services.AddSingleton<IDatabaseConnectionService, DatabaseConnectionService>();

                services.AddHostedService<Worker>();
            });
}