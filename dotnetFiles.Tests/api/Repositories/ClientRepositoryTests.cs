using Api.Repositories;
using Api.Data;
using Xunit;
using Npgsql;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using Moq;
using Api.Models;




public class ClientRepositoryTests
{
    //////////////
    /// NAMING ///  MethodName_StateUnderTest_ExpectedReuls
    //////////////


    // Test methods must always return
    // void disregarding the actual method return type

    [Fact]
    public void ClientRepository_CanBeInstantiated_WithIDbContext()
    {
        // Arrange
        var mockDb = new Mock<IDbContext>();

        // Act
        var repository = new ClientRepository(mockDb.Object);

        // Assert
        Assert.NotNull(repository);
    }

    // NOTE: Testing GetAllClients() requires a real database connection
    // because NpgsqlConnection cannot be mocked.
    // For full repository testing, use integration tests with a test database.
    
    // Example integration test approach (requires test DB):
    // [Fact]
    // public void GetAllClients_WithTestDb_ReturnsClients()
    // {
    //     var dbContext = new DbContext(); // or test connection string
    //     var repository = new ClientRepository(dbContext);
    //     var result = repository.GetAllClients();
    //     Assert.NotNull(result);
    // }

}
