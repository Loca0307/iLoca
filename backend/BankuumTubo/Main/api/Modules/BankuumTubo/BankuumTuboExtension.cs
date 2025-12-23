using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Api.Repositories;
using Api.Services;



namespace Api.Modules.BankuumTubo;

public static class BankuumTuboExtension
{
    // Module to import the project in Program.cs, and simplify structure there
    public static IServiceCollection BankuumTuboModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Repositories
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        // Services
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITransactionService, TransactionService>();

        return services;
    }
}
