using Api.Data;
using Api.Models;
using Npgsql;


namespace Api.Repositories;

public class LoginRepository : ILoginRepository
{
    private readonly IDbContext _dbcontext;


    public LoginRepository(IDbContext dbContext)
    {
        _dbcontext = dbContext;
    }


    public void InsertLogin(Login login)
    {
        using var conn = _dbcontext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            @"INSERT INTO logins (Email, Password)
            VALUES (@email, @password)
            ON CONFLICT (Email) DO NOTHING", conn);

    
        cmd.Parameters.AddWithValue("email", login.Email);
        cmd.Parameters.AddWithValue("password", login.Password);

        // Actually execute the query
        cmd.ExecuteNonQuery();

    }
}