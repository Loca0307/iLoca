using Xunit;
using Moq;
using Api.Services;
using Api.Models;
using System.Collections.Generic;
using Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Api.DTO;
using Microsoft.AspNetCore.Http.HttpResults;


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
        // "OkObjectResult" is a special type for controllers response 
        // that check the reponse status and object.
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var dtos = Assert.IsType<List<ClientDTO>>(okResult.Value);
        Assert.Single(dtos);
        Assert.Equal(1, dtos[0].ClientId);
        Assert.Equal("John", dtos[0].FirstName);

        mockServ.Verify(s => s.GetAllClients(), Times.Once);
    }

    [Fact]
    public void InsertClient_WhenCalled_InsertsAClient()
    {
        var mockServ = new Mock<IClientService>();

        var client = new Client
        {
            ClientId = 10,
            FirstName = "Pietro",
            LastName = "Bianchi",
            Email = "p@1",
            Phone = "675858",
            Iban = "444",
            Balance = 10m
        };
        // InsertClient is a void method on the service; set up a callback to allow verification
        mockServ.Setup(s => s.InsertClient(It.IsAny<Client>()))
                .Callback<Client>(c => {
                    // simulate DB assigning an ID if needed (optional)
                    // leave client as-is for this test
                });

        var controller = new ClientController(mockServ.Object);

        // Actually call the method
        ActionResult<ClientDTO> actionResult = controller.InsertClient(client);

        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var dto = Assert.IsType<ClientDTO>(okResult.Value);
        // ASSERT - returned DTO should reflect the input client values
        Assert.Equal(10, dto.ClientId);
        Assert.Equal("Pietro", dto.FirstName);
        Assert.Equal("Bianchi", dto.LastName);
        Assert.Equal("p@1", dto.Email);
        Assert.Equal("444", dto.Iban);
    Assert.Equal(10m, dto.Balance);

        mockServ.Verify(s => s.InsertClient(It.IsAny<Client>()), Times.Once);
    }
}