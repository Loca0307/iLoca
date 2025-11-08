using Api.Data;
using Api.Models;
using BCrypt.Net;
using Npgsql;


namespace Api.Repositories;

public class LoginRepository : ILoginRepository
{
    private readonly IDbContext _dbContext;


    public LoginRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public List<Login> GetAllLogins()
    {
        var logins = new List<Login>();
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
        @"SELECT LoginID, Email, Password FROM logins", conn
        );
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            logins.Add(new Login
            {
                LoginId = reader.GetInt32(0),
                Email = reader.GetString(1),
                Password = reader.GetString(2)
            });
        }

        return logins;
    }

    public void InsertLogin(Login login)
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            @"INSERT INTO logins (Email, Password)
            VALUES (@email, @password)
            ON CONFLICT (Email) DO NOTHING", conn);

        // Define the Hashed version of the password to be saved in the database
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(login.Password);

        cmd.Parameters.AddWithValue("email", login.Email);
        cmd.Parameters.AddWithValue("password", hashedPassword);

        // Actually execute the query
        cmd.ExecuteNonQuery();
    }

    public void DeleteLogin(Login login)
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            @"DELETE FROM logins
            WHERE LoginId = @id", conn);

        cmd.Parameters.AddWithValue("id", login.LoginId);
        cmd.ExecuteNonQuery();
    }
}