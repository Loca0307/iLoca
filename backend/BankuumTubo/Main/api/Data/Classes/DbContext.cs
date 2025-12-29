using System;
using DotNetEnv;
using Npgsql;

namespace Api.Data;

public class DbContext : IDbContext
{
    private readonly string _connectionString;

    public DbContext()
    {
        // Load environment variables from .env
        Env.Load();

        string host = Environment.GetEnvironmentVariable("DB_HOST") ?? "db";
        int port = int.Parse(Environment.GetEnvironmentVariable("DB_PORT") ?? "5432");
        string database = Environment.GetEnvironmentVariable("DB_DATABASE") ?? "app_db";
        string username = Environment.GetEnvironmentVariable("DB_USERNAME") ?? "postgres";
        string password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "postgres";

        // Determine SSL mode depending on the host
        string sslMode;
        if (host == "db" || host == "localhost") // local Docker DB
        {
            sslMode = "Disable";
        }
        else // remote hosted DB
        {
            sslMode = "Require;Trust Server Certificate=true";
        }

        _connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode={sslMode};";
    }

    public NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
