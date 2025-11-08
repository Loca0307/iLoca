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

}