using Api.Data;
using Api.Models;
using Npgsql;

namespace Api.Repositories;


public class TransactionRepository
{
    private readonly IDbContext _dbContext;

    public TransactionRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // RETURN ALL THE TRANSACTIONS
    public List<Transaction> GetAllTransactions()
    {
        var transactions = new List<Transaction>();

        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
        @"SELECT TransactionId, Sender, Receiver, 
        Amount, DateTime, Reason FROM transactions", conn
        );

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            transactions.Add(
                new Transaction
                {
                    TransactionId = reader.GetInt32(0),
                    Sender = reader.GetString(1),
                    Receiver = reader.GetString(2),
                    Amount = reader.GetInt32(3),
                    DateTime = reader.GetDateTime(4),
                    Reason = reader.GetString(5)

                }
            );
        }

        return transactions;
    }

    public void InsertTransaction(Transaction transaction)
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            @"INSERT INTO transactions (Sender, Receiver,
            Amount, DateTime, Reason)
            VALUES (@sender, @receiver, @amount, @dateTime, @reason)", conn);

        // Define the Transaction values
        cmd.Parameters.AddWithValue("sender", transaction.Sender);
        cmd.Parameters.AddWithValue("receiver", transaction.Receiver);
        cmd.Parameters.AddWithValue("amount", transaction.Amount);
        cmd.Parameters.AddWithValue("dateTime", transaction.DateTime);
        cmd.Parameters.AddWithValue("Reason", transaction.Reason);

        cmd.ExecuteNonQuery();

    }

    public void DeleteTransaction(Transaction transaction)
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
        @"DELETE FROM transactions
        WHERE TransactionId = @id"
        , conn);

        cmd.Parameters.AddWithValue("id", transaction.TransactionId);
        cmd.ExecuteNonQuery();
    }
    
    public void DeleteAllTransactions(){
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
        @"DELETE FROM transactions"
        , conn);

        cmd.ExecuteNonQuery();
    }

}