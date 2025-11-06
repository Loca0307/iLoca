using Npgsql;
using System;

namespace Api.Data;

public class DbContext
{
    private readonly string _connectionString;

    public DbContext()
    {
        string host = "bankuumtubo-bankuumtubo.f.aivencloud.com";
        int port = 28017;
        string database = "bankuumdb";
        string username = "avnadmin";
        string password = "AVNS_u-FjY5aszGHtMkGPQmY";

        _connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;";
    }

    public NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
    