using Npgsql;
using System;
using DotNetEnv;

namespace Api.Data;

public class DbContext : IDbContext
{
    private readonly string _connectionString;

    public DbContext() 
    {
        // Load the .env file
        Env.Load();

        string host = Environment.GetEnvironmentVariable("DB_HOST") 
                      ?? throw new Exception("DB_HOST environment variable is not set");

        int port = int.Parse(Environment.GetEnvironmentVariable("DB_PORT")!);

        string database = Environment.GetEnvironmentVariable("DB_DATABASE") 
                          ?? throw new Exception("DB_DATABASE environment variable is not set");

        string username = Environment.GetEnvironmentVariable("DB_USERNAME") 
                          ?? throw new Exception("DB_USERNAME environment variable is not set");

        string password = Environment.GetEnvironmentVariable("DB_PASSWORD") 
                          ?? throw new Exception("DB_PASSWORD environment variable is not set");

        // Build the connection string
        _connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;";
    }

    public NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
