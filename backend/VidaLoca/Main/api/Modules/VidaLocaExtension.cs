using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Api.Repositories;
using Api.Services;
using Api.Data;



namespace Api.Modules.VidaLoca;

public static class VidaLocaExtension
{
    // Module to import the project in Program.cs, and simplify structure there
    public static IServiceCollection VidaLocaModule(this IServiceCollection services, IConfiguration configuration)
    {
    /*    // DB layer
        services.AddScoped<IDbContext, DbContext>(); */

        // Repositories
        services.AddScoped<IAccountRepository, AccountRepository>();


        // Services
        services.AddScoped<IAccountService, AccountService>();

        return services;
    }
}
