using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Api.Repositories;
using Api.Services;



namespace Api.Modules.VidaLoca;

public static class VidaLocaExtension
{
    // Module to import the project in Program.cs, and simplify structure there
    public static IServiceCollection VidaLocaModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Repositories
        services.AddScoped<IClientRepository, ClientRepository>();


        // Services
        services.AddScoped<IClientService, ClientService>();

        return services;
    }
}
