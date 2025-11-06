using Xunit;
using Moq;
using Api.Services;
using Api.Models;
using System.Collections.Generic;
using Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Api.DTO;


public class ClientControllerTests
{

    //////////////
    /// NAMING ///  MethodName_StateUnderTest_ExpectedReuls
    //////////////
    

    // Test methods must always return
    // void disregarding the actual method return type
    
    [Fact]
    public void GetAllCLients_WhenCalled_ReturnsAllCLients()
    {
        // Mocks the Interface that the concrete method calls for testing.
        // It simulates a ClientService that returns them a List<CLient> like
        // the service "GetAllCLients()" method.
        var mockServ = new Mock<IClientService>();
        mockServ.Setup(s => s.GetAllClients()).Returns(new List<Client>
        {
            new Client { ClientId = 1, FirstName = "John", LastName = "Doe", Email = "j@x.com", Phone = "123" }
        });

        // Create instance of the actual class to test
        var controller = new ClientController(mockServ.Object);

        // ACTUAL TESTING METHOD
        ActionResult<List<ClientDTO>> actionResult = controller.GetAllClients();

        // ASSERT
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var dtos = Assert.IsType<List<ClientDTO>>(okResult.Value);
        Assert.Single(dtos);
        Assert.Equal(1, dtos[0].ClientId);
        Assert.Equal("John", dtos[0].FirstName);

        mockServ.Verify(s => s.GetAllClients(), Times.Once);
    }
    
    
}