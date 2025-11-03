using Microsoft.AspNetCore.Mvc;
using Api.Models;
using Api.Services;
using System.Collections.Generic;

namespace Api.Controllers;


// Defines this controller file in the "ApiController" group
// it automatically defines "api/client" as the route to manage
[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly ClientService _clientService;

    public ClientController(ClientService clientService)
    {
        _clientService = clientService;
    }

    // this line defines which CRUD method to execute 
    [HttpGet("{showClients}")]
    public ActionResult<List<Client>> GetAllClients()
    {       
        // From the controller you call the service methods that use database and logic
        var clients = _clientService.GetAllClients();
        return Ok(clients);
    }

    [HttpPost("{addClient}")]
    public IActionResult AddClient([FromBody] Client client)
    {
        _clientService.AddClient(client);
        return Ok(new { message = "Client added successfully" });
    }
}
