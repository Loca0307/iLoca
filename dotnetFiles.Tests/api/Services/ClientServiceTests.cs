using Xunit;
using Moq;
using Api.Services;
using Api.Repositories;
using Api.Models;
using System.Collections.Generic;

public class ClientServiceTests
{
    //////////////
    /// NAMING ///  MethodName_StateUnderTest_ExpectedReuls
    //////////////
 

    // Test methods must always return
    // void disregarding the actual method return type

    [Fact] 
    public void GetAllClients_WhenCalled_ReturnsAllClients()
    {
       
        var mockRepo = new Mock<IClientRepository>();
        mockRepo.Setup(r => r.GetAllClients()).Returns(new List<Client>
        {
            new Client { ClientId = 1, FirstName = "John", LastName = "Doe" }
        });
        var service = new ClientService(mockRepo.Object);

        // ACTUAL TESTING METHOD
        var result = service.GetAllClients();

        // ASSERT
        Assert.Single(result);
        Assert.Equal("John", result[0].FirstName);
    }
}