using Api.Data;
using Api.Models;
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

        cmd.Parameters.AddWithValue("email", login.Email);
        cmd.Parameters.AddWithValue("password", login.Password); // Password should already be hashed

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

    public Login? GetLoginByEmail(string email)
    {
         using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            @"SELECT LoginId, Email, Password FROM logins
            WHERE Email = @email", conn
        );

        cmd.Parameters.AddWithValue("email", email);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Login
            {
                LoginId = reader.GetInt32(0),
                Email = reader.GetString(1),
                Password = reader.GetString(2)
            };
        }

        return null;

    }
    
    public void DeleteAllLogins(){
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            @"DELETE FROM logins",
            conn
        );

        cmd.ExecuteNonQuery();
    }
}