using Microsoft.AspNetCore.Mvc;
using Api.Models;
using Api.DTO;
using Api.Services;
using System.Collections.Generic;
using System.Linq;

namespace Api.Controllers;


// Defines this controller file in the "ApiController" group
// it automatically defines "api/client" as the route to manage
[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }


    // RETRIEVE ALL CLIENTS FROM THE DATABASE
    // this line defines which CRUD method to execute 
    [HttpGet("ShowClients")]
    public ActionResult<List<ClientDTO>> GetAllClients() {
        // From the controller you call the service methods that use database and logic
        var clients = _clientService.GetAllClients();
        
        // Map Client models to ClientDTOs
        var clientDTOs = clients.Select(c => new ClientDTO
        {
            ClientId = c.ClientId,
            FirstName = c.FirstName,
            LastName = c.LastName,
            Email = c.Email,
            Phone = c.Phone
        }).ToList();
        
        return Ok(clientDTOs);
    }


    // ADD A CLIENT TO THE DATABASE
    [HttpPost("InsertClient")]
    public ActionResult<ClientDTO> InsertClient([FromBody] Client client)
    {
        _clientService.InsertClient(client);
        
        // Map the inserted client to DTO and return it
        // return clientDTO so that frontend already has ID without new request
        var clientDTO = new ClientDTO
        {
            ClientId = client.ClientId,
            FirstName = client.FirstName,
            LastName = client.LastName,
            Email = client.Email,
            Phone = client.Phone
        };
        
        return Ok(clientDTO);
    }


    // REMOVE A CLIENT FROM THE DATABASE
    [HttpDelete("DeleteClient")]
    public ActionResult DeleteClient(Client client)
    {
        _clientService.DeleteClient(client);
        return Ok(new { message = "Client has been removed"});
    }
    
}
