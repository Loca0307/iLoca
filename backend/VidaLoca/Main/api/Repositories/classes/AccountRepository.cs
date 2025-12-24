using System;
using Npgsql;
using Api.Data;
using Api.Models;

namespace Api.Repositories;

public class AccountRepository : IAccountRepository{

    private readonly IDbContext _dbContext;

    public AccountRepository(IDbContext dbContext) {
        _dbContext = dbContext;
    }
    
    public List<Account> GetAllAccounts() {
        
        var accounts = new List<Account>();
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
        @"SELECT account_id, email, password, user_name, client_id 
        FROM ""VidaLoca"".accounts", 
        conn);

        using var reader = cmd.ExecuteReader();

        while (reader.Read()) {
            accounts.Add(new Account{
                AccountId = reader.GetInt32(0),
                Email = reader.GetString(1),
                Password = reader.GetString(2),
                Username = reader.GetString(3),
                ClientId = reader.GetInt32(4)
            });
        }

        return accounts;
    }

}