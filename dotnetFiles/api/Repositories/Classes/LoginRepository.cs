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
    @"SELECT LoginId, Email, Password, Username, ClientId FROM logins", conn
    );
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            logins.Add(new Login
            {
                LoginId = reader.GetInt32(0),
                Email = reader.GetString(1),
                Password = reader.GetString(2),
                Username = reader.IsDBNull(3) ? "" : reader.GetString(3),
                ClientId = reader.IsDBNull(4) ? 0 : reader.GetInt32(4)
            });
        }

        return logins;
    }

    public void InsertLogin(Login login)
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            @"INSERT INTO logins (Email, Password, Username, ClientId)
            VALUES (@email, @password, @username, @clientId)
            ON CONFLICT (Email) DO NOTHING", conn);

        cmd.Parameters.AddWithValue("email", login.Email);
        cmd.Parameters.AddWithValue("password", login.Password); // Password should already be hashed
        cmd.Parameters.AddWithValue("username", login.Username);
        cmd.Parameters.AddWithValue("clientId", login.ClientId);
        

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

    //  RETURN IF PRESENT THE LOGIN OF THE GIVEN EMAIL
    public Login? GetLoginByEmail(string email)
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            @"SELECT LoginId, Email, Password, Username, ClientId FROM logins
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
                Password = reader.GetString(2),
                Username = reader.GetString(3),
                ClientId = reader.GetInt32(4)
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