using Api.Models;
using Api.Data;
using Npgsql;
using System.Collections.Generic;

namespace Api.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly IDbContext _dbContext;

    public ClientRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    // RETRIEVE ALL CLIENTS FROM THE DATABASE
    public List<Client> GetAllClients()
    {
        var clients = new List<Client>();

        // "using" is used so that the object gets automatically deallocated after use
        using var conn = _dbContext.GetConnection();
        conn.Open();

        // For "Get" returning request you need to "Read" 
        using var cmd = new NpgsqlCommand("SELECT client_id, first_name, last_name, email, phone FROM clients", conn);
        using var reader = cmd.ExecuteReader();

        // to structure the read data
        while (reader.Read())
        {
            clients.Add(new Client
            {
                // To trasform the data from the actual DB return to a JSON
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
        using var conn = _dbContext.GetConnection();
        conn.Open();
        
        // Define the SQL query
        using var cmd = new NpgsqlCommand(
            @"INSERT INTO clients (first_name, last_name, email, phone,
            balance)
              VALUES (@first, @last, @email, @phone, @balance)
              ON CONFLICT (email) DO NOTHING", conn);

        // Define actual parameters
        cmd.Parameters.AddWithValue("first", client.FirstName);
        cmd.Parameters.AddWithValue("last", client.LastName);
        cmd.Parameters.AddWithValue("email", client.Email);
        cmd.Parameters.AddWithValue("phone", client.Phone);
        cmd.Parameters.AddWithValue("balance", client.Balance);

        // To actually run the query 
        cmd.ExecuteNonQuery();
    }

    public void DeleteClient(Client client)
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            @"DELETE FROM clients
            WHERE client_id = @id", conn);

        cmd.Parameters.AddWithValue("id", client.ClientId);
        cmd.ExecuteNonQuery();
    }


    public void DeleteAllClients() 
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
                @"DELETE FROM clients", conn
        );

        cmd.ExecuteNonQuery();
    }
    
}
