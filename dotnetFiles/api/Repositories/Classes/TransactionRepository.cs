using Api.Data;
using Api.Models;
using Npgsql;

namespace Api.Repositories;


public class TransactionRepository : ITransactionRepository
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
        @"SELECT transactionId, sender_email, receiver_iban, 
        amount, dateTime, reason FROM transactions", conn
        );

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            transactions.Add(
                new Transaction
                {
                    TransactionId = reader.GetInt32(0),
                    SenderEmail = reader.GetString(1),
                    ReceiverIban = reader.GetString(2),
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
            @"INSERT INTO transactions (sender_email, receiver_iban,
            amount, dateTime, reason)
            VALUES (@sender, @receiver, @amount, @dateTime, @reason)", conn);

        // Define the Transaction values
        cmd.Parameters.AddWithValue("sender", transaction.SenderEmail);
        cmd.Parameters.AddWithValue("receiver", transaction.ReceiverIban);
        cmd.Parameters.AddWithValue("amount", transaction.Amount);
        cmd.Parameters.AddWithValue("dateTime", transaction.DateTime);
        cmd.Parameters.AddWithValue("reason", transaction.Reason);

        cmd.ExecuteNonQuery();
    }

    public void DeleteTransaction(Transaction transaction)
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
        @"DELETE FROM transactions
        WHERE transactionId = @id"
        , conn);

        cmd.Parameters.AddWithValue("id", transaction.TransactionId);
        cmd.ExecuteNonQuery();
    }

    public void DeleteAllTransactions()
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
        @"DELETE FROM transactions"
        , conn);

        cmd.ExecuteNonQuery();
    }
    

}