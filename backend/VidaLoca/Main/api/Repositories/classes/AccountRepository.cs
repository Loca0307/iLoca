using Api.Data;
using Api.DTO;
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

        while (reader.Read())
        {
            accounts.Add(new Account
            {
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


    public void DeleteAllAccounts()
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            @"DELETE FROM ""VidaLoca"".accounts",
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



    public decimal? GetBalance(int accountId)
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(@"SELECT balance FROM ""VidaLoca"".accounts WHERE account_id = @id", conn);
        cmd.Parameters.AddWithValue("id", accountId);
        var res = cmd.ExecuteScalar();
        if (res == null || res == DBNull.Value) return null;
        return Convert.ToDecimal(res);
    }

    public bool TransferFromBankToVida(int accountId, string bankIban, decimal amount)
    {
        if (amount <= 0) return false;

        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var tx = conn.BeginTransaction();
        try
        {
            // 1) Lock bank client by IBAN
            int bankClientId;
            decimal bankBalance;
            using (var cmdBank = new NpgsqlCommand(@"SELECT client_id, balance FROM ""BankuumTubo"".clients WHERE iban = @iban FOR UPDATE", conn, tx))
            {
                cmdBank.Parameters.AddWithValue("iban", bankIban);

                using var r = cmdBank.ExecuteReader();
                if (!r.Read()) { tx.Rollback(); return false; }
                bankClientId = r.GetInt32(0);
                bankBalance = r.GetDecimal(1);
            }

            if (bankBalance < amount)
            {
                tx.Rollback();
                return false; // insufficient bank funds
            }

            // 2) Lock VidaLoca account
            using (var cmdVida = new NpgsqlCommand(@"SELECT balance FROM ""VidaLoca"".accounts WHERE account_id = @id FOR UPDATE", conn, tx))
            {
                cmdVida.Parameters.AddWithValue("id", accountId);

                var res = cmdVida.ExecuteScalar();
                if (res == null || res == DBNull.Value) { tx.Rollback(); return false; }
            }

            // 3) Subtract from bank
            using (var cmdUpdateBank = new NpgsqlCommand(@"UPDATE ""BankuumTubo"".clients SET balance = balance - @amt WHERE client_id = @id", conn, tx))
            {
                cmdUpdateBank.Parameters.AddWithValue("amt", amount);
                cmdUpdateBank.Parameters.AddWithValue("id", bankClientId);
                cmdUpdateBank.ExecuteNonQuery();
            }

            // 4) Add to VidaLoca
            using (var cmdUpdateVida = new NpgsqlCommand(@"UPDATE ""VidaLoca"".accounts SET balance = balance + @amt WHERE account_id = @id", conn, tx))
            {
                cmdUpdateVida.Parameters.AddWithValue("amt", amount);
                cmdUpdateVida.Parameters.AddWithValue("id", accountId);
                cmdUpdateVida.ExecuteNonQuery();
            }

            tx.Commit();
            return true;
        }
        catch
        {
            try { tx.Rollback(); } catch { }
            throw;
        }
    }

    public BankClientInfo? GetBankClientByIban(string iban)
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(@"SELECT client_id, first_name, last_name, email, iban, balance FROM ""BankuumTubo"".clients WHERE iban = @iban", conn);
        cmd.Parameters.AddWithValue("iban", iban);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        return new BankClientInfo
        {
            ClientId = reader.GetInt32(0),
            FirstName = reader.GetString(1),
            LastName = reader.GetString(2),
            Email = reader.GetString(3),
            Iban = reader.GetString(4),
            Balance = reader.GetDecimal(5),
        };
    }
}