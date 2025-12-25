using Api.Data;
using Api.Models;
using Npgsql;


namespace Api.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IDbContext _dbContext;


    public AccountRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public List<Account> GetAllAccounts()
    {
        // Define Accounts list to be returned afterwrds
        var accounts = new List<Account>(); 
        using var conn = _dbContext.GetConnection();
        conn.Open();

    using var cmd = new NpgsqlCommand(
    @"SELECT account_id, email, password, user_name 
    FROM ""VidaLoca"".accounts", 
    conn);

        using var reader = cmd.ExecuteReader();

        while (reader.Read()) {
            accounts.Add(new Account {
                AccountId = reader.GetInt32(0),
                Email = reader.GetString(1),
                Password = reader.GetString(2),
                Username = reader.GetString(3),
            });
        }

        return accounts;
    }

    public void InsertAccount(Account account)
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            @"INSERT INTO ""VidaLoca"".accounts (email, password, user_name) 
            VALUES (@email, @password, @username)", 
            conn);

        cmd.Parameters.AddWithValue("email", account.Email);
        cmd.Parameters.AddWithValue("password", account.Password); // Password should already be hashed
        cmd.Parameters.AddWithValue("username", account.Username);
        

        cmd.ExecuteNonQuery();
    }

    public void DeleteAccount(Account account)
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            @"DELETE FROM ""VidaLoca"".accounts 
            WHERE account_id = @id", 
            conn);

        cmd.Parameters.AddWithValue("id", account.AccountId);
        cmd.ExecuteNonQuery();
    }

    
    public void DeleteAllAccounts(){
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            @"DELETE FROM ""VidaLoxa"".accounts",
            conn);

        cmd.ExecuteNonQuery();
    }

    // Method that returns only the username of a Account to set the localStorage variable
    public string? GetUsernameByEmail(string email)
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            @"SELECT user_name FROM ""VidaLoca"".accounts 
            WHERE email = @email", 
            conn);

        cmd.Parameters.AddWithValue("email", email);


        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return reader.GetString(0);

        }

        return null;
    }

    public Account? GetAccountByEmail(string email)
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            @"SELECT account_id, email, password, user_name 
            FROM ""VidaLoca"".accounts WHERE email = @email", 
            conn);

        cmd.Parameters.AddWithValue("email", email);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Account
            {
                AccountId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                Email = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                Password = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                Username = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
            };
        }

        return null;
    }
}