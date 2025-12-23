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
    @"SELECT login_id, email, password, user_name, client_id FROM logins", conn
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
            @"INSERT INTO logins (email, password, user_name, client_id)
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
            WHERE login_id = @id", conn);

        cmd.Parameters.AddWithValue("id", login.LoginId);
        cmd.ExecuteNonQuery();
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

    // Method that returns only the username of a login to set the localStorage variable
    public string? GetUsernameByEmail(string email)
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            @"SELECT user_name FROM logins
            WHERE email = @email", conn
        );

        cmd.Parameters.AddWithValue("email", email);


        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return reader.GetString(0);

        }

        return null;
    }

    public Login? GetLoginByEmail(string email)
    {
        using var conn = _dbContext.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            @"SELECT login_id, email, password, user_name, client_id FROM logins
            WHERE email = @email", conn
        );

        cmd.Parameters.AddWithValue("email", email);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Login
            {
                LoginId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                Email = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                Password = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                Username = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                ClientId = reader.IsDBNull(4) ? 0 : reader.GetInt32(4)
            };
        }

        return null;
    }

}