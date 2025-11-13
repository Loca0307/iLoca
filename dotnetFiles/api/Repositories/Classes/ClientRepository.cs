using Api.Models;
using Api.Data;
using Npgsql;
using System.Collections.Generic;
using System;

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
        using var cmd = new NpgsqlCommand("SELECT client_id, first_name, last_name, email, phone, iban, balance FROM clients", conn);
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
                Phone = reader.GetString(4),
                Iban = reader.GetString(5),
                Balance = reader.GetDecimal(6)
            });
        }

        return clients;
    }

    public void InsertClient(Client client)
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();
        

        // Define the SQL query (include iban and balance)
        using var cmd = new NpgsqlCommand(
            "INSERT INTO clients (first_name, last_name, email, phone, iban, balance) VALUES (@first, @last, @email, @phone, @iban, @balance) ON CONFLICT (email) DO NOTHING",
            conn);

        // Define actual parameters
        cmd.Parameters.AddWithValue("first", client.FirstName);
        cmd.Parameters.AddWithValue("last", client.LastName);
        cmd.Parameters.AddWithValue("email", client.Email);
        cmd.Parameters.AddWithValue("phone", client.Phone);
        cmd.Parameters.AddWithValue("iban", client.Iban);
        cmd.Parameters.AddWithValue("balance", 1000);

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

    //  RETURN IF PRESENT THE LOGIN OF THE GIVEN EMAIL
    public Client? GetClientByEmail(string email)
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            @"SELECT client_id, first_name, last_name, email, phone, iban, balance 
            FROM clients
            WHERE Email = @email", conn
        );

        cmd.Parameters.AddWithValue("email", email);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Client
            {
                ClientId = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Email = reader.GetString(3),
                Phone = reader.GetString(4),
                Iban = reader.GetString(5),
                Balance = reader.GetDecimal(6)
            };
        }

        return null;
    }

    // RETURN A CLIENT(if found) THAT MATCHES WITH THE GIVEN IBAN
    public Client? GetClientByIban(string iban)
    {

        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            @"SELECT client_id, first_name, last_name, email, phone, iban, balance 
            FROM clients 
            WHERE Iban = @iban",
            conn);

        cmd.Parameters.AddWithValue("iban", iban);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Client
            {
                ClientId = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Email = reader.GetString(3),
                Phone = reader.GetString(4),
                Iban = reader.GetString(5),
                Balance = reader.GetDecimal(6)
            };
        }

        return null;
    }

    public Boolean CheckBalance(Client client, decimal amount)
    {
        // Null client => cannot check
        if (client == null)
            return false;


        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            @"SELECT balance FROM clients WHERE client_id = @id", conn);

        cmd.Parameters.AddWithValue("id", client.ClientId);

        var result = cmd.ExecuteScalar();

        if (result == null || result == DBNull.Value)
            return false;

        var balance = Convert.ToDecimal(result);
        return balance >= amount;
    }

    public void EditBalance(Client client, decimal amount)
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
        @"UPDATE clients
        SET balance = balance + @amount
        WHERE client_id = @clientId;"
        , conn);

        cmd.Parameters.AddWithValue("clientId", client.ClientId);
        cmd.Parameters.AddWithValue("amount", amount);
        cmd.ExecuteNonQuery();
    }
    
    // Atomic transfer: lock sender and receiver rows, verify sender balance, update balances and insert transaction
    public void TransferAndRecordTransaction(Client sender, Client receiver, Transaction transaction)
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        var amount = transaction.Amount;

        // Used to start a atomic transaction with mutiple queries
        // so either all of them commit or they all rollback and abort
        using var tx = conn.BeginTransaction();
        try
        {
            // Lock both client rows in a consistent order (by client_id) to avoid deadlocks
            var firstId = Math.Min(sender.ClientId, receiver.ClientId);
            var secondId = Math.Max(sender.ClientId, receiver.ClientId);

            // Lock those client so no other update can happen to them during the transaction processx
            using (var cmdLock = new NpgsqlCommand("SELECT client_id, balance FROM clients WHERE client_id IN (@id1, @id2) FOR UPDATE", conn, tx))
            {
                cmdLock.Parameters.AddWithValue("id1", firstId);
                cmdLock.Parameters.AddWithValue("id2", secondId);
                using var reader = cmdLock.ExecuteReader();
                // Read and ignore results; rows are locked for this transaction
                while (reader.Read()) { /* no-op */ }
            }

            // Re-check sender balance from DB to avoid stale reads
            decimal dbSenderBalance;
            using (var cmdGet = new NpgsqlCommand("SELECT balance FROM clients WHERE client_id = @id", conn, tx))
            {
                cmdGet.Parameters.AddWithValue("id", sender.ClientId);
                var res = cmdGet.ExecuteScalar();
                if (res == null || res == DBNull.Value)
                    throw new InvalidOperationException("Sender not found during transfer.");

                dbSenderBalance = Convert.ToDecimal(res);
            }

            // Re-check that the sender has enough credit on the balance
            if (dbSenderBalance < amount)
            {
                throw new InvalidOperationException("You don't have enough credit on your account for this transaction.");
            }

            // Update sender balance (subtract)
            using (var cmdUpdateSender = new NpgsqlCommand("UPDATE clients SET balance = balance - @amount WHERE client_id = @id", conn, tx))
            {
                cmdUpdateSender.Parameters.AddWithValue("amount", amount);
                cmdUpdateSender.Parameters.AddWithValue("id", sender.ClientId);
                cmdUpdateSender.ExecuteNonQuery();
            }

            // Update receiver balance (add)
            using (var cmdUpdateReceiver = new NpgsqlCommand("UPDATE clients SET balance = balance + @amount WHERE client_id = @id", conn, tx))
            {
                cmdUpdateReceiver.Parameters.AddWithValue("amount", amount);
                cmdUpdateReceiver.Parameters.AddWithValue("id", receiver.ClientId);
                cmdUpdateReceiver.ExecuteNonQuery();
            }

            // Insert transaction record
            using (var cmdInsert = new NpgsqlCommand(@"INSERT INTO transactions (sender_email, receiver_iban, amount, dateTime, reason) VALUES (@sender, @receiver, @amount, @dateTime, @reason)", conn, tx))
            {
                cmdInsert.Parameters.AddWithValue("sender", transaction.SenderEmail);
                cmdInsert.Parameters.AddWithValue("receiver", transaction.ReceiverIban);
                cmdInsert.Parameters.AddWithValue("amount", transaction.Amount);
                cmdInsert.Parameters.AddWithValue("dateTime", transaction.DateTime);
                cmdInsert.Parameters.AddWithValue("reason", transaction.Reason ?? string.Empty);
                cmdInsert.ExecuteNonQuery();
            }

            tx.Commit();
        }
        catch
        {
            // If any SQL statement fails, it catches it
            // and rolls back everything
            try { tx.Rollback(); } catch { }
            throw;
        }
    }
    
}
