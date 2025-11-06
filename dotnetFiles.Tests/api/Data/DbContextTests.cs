using Xunit;
using Api.Data;

public class DbContextTests
{

     //////////////
    /// NAMING ///  MethodName_StateUnderTest_ExpectedReuls
    //////////////
    

    // Test methods must always return
    // void disregarding the actual method return type

    [Fact]
    public void GetConnection_ReturnsNpgsqlConnection_WithConnectionStringAndClosedState()
    {
        // Create instance of the actual class to test

        var context = new DbContext();

        // ACTUAL TESTING METHOD
        var conn = context.GetConnection();

        // ASSERT
        Assert.NotNull(conn);
        Assert.False(string.IsNullOrWhiteSpace(conn.ConnectionString));
        // Ensure it is not open by default (we didn't call Open())
        Assert.Equal(System.Data.ConnectionState.Closed, conn.State);
        
        // Optional: check a couple of expected keys exist in the connection string
        Assert.Contains("Host=", conn.ConnectionString);
        Assert.Contains("Database=", conn.ConnectionString);
        Assert.Contains("Username=", conn.ConnectionString);
    }
}
