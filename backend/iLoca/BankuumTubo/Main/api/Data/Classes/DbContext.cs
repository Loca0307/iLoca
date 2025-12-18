using Npgsql;
using System;
using DotNetEnv;

namespace Api.Data;

public class DbContext : IDbContext
{
    private readonly string _connectionString;

    public DbContext() 
    {
        string host = "pg-2325aa38-loca2gaming-18ad.l.aivencloud.com";
        int port = 13765;
        string database = "defaultdb";
        string username = "avnadmin";


        // Load .env file for password
        Env.Load();

        string password = "AVNS_6GtbsR1hQPaErNpfB2O";
                
            // Get the password from an environment variable instead of hardcoding it
            //Environment.GetEnvironmentVariable("DB_PASSWORD") ?? throw new Exception("DB_PASSWORD environment variable is not set");

        _connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;";
    }

    public NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
