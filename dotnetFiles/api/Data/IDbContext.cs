using Npgsql;

namespace Api.Data;


public interface IDbContext
{

    public NpgsqlConnection GetConnection();

}