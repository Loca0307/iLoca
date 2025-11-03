using Api.Models;
using Api.DBContext;
using Npgsql;
using System.Collections.Generic;

namespace Api.Repositories;

public class ClientRepository
{
    private readonly AppDBContext _dbService;

    public ClientRepository(AppDBContext dbService)
    {
        _dbService = dbService;
    }

    public List<Client> GetAllClients()
    {
        var clients = new List<Client>();

        using var conn = _dbService.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand("SELECT client_id, first_name, last_name, email, phone FROM clients", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            clients.Add(new Client
            {
                ClientId = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Email = reader.GetString(3),
                Phone = reader.GetString(4)
            });
        }

        return clients;
    }

    public void InsertClient(Client client)
    {
        using var conn = _dbService.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            @"INSERT INTO clients (first_name, last_name, email, phone)
              VALUES (@first, @last, @email, @phone)
              ON CONFLICT (email) DO NOTHING", conn);

        cmd.Parameters.AddWithValue("first", client.FirstName);
        cmd.Parameters.AddWithValue("last", client.LastName);
        cmd.Parameters.AddWithValue("email", client.Email);
        cmd.Parameters.AddWithValue("phone", client.Phone);

        cmd.ExecuteNonQuery();
    }
}
