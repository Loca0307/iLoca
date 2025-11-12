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
[Route("/[controller]")]
public class ClientController : ControllerBase
{
    // Define the clientService to use for the controller
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }


    // RETRIEVE ALL CLIENTS FROM THE DATABASE
    // ActionResult is a specific type that is returned from 
    // the controllers with request status and object
    [HttpGet("ShowClients")] // this line defines which CRUD method to execute.
    public ActionResult<List<ClientDTO>> GetAllClients() {
        // From the controller you call the service methods that use database and logic
        var clients = _clientService.GetAllClients();
        
        // Map Client model to ClientDTOs
        var clientDTOs = clients.Select(c => new ClientDTO
        {
            ClientId = c.ClientId,
            FirstName = c.FirstName,
            LastName = c.LastName,
            Email = c.Email,
            Phone = c.Phone,
            Iban = c.Iban
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
            Phone = client.Phone,
            Iban = client.Iban,
            Balance = client.Balance
        };
        
        return Ok(clientDTO);
    }


    // REMOVE A CLIENT FROM THE DATABASE
    [HttpDelete("DeleteClient")]
    public ActionResult DeleteClient(Client client)
    {
        _clientService.DeleteClient(client);
        return Ok(new { message = "Client has been successfully removed" });
    }


    // DELETE ALL CLIENTS
    [HttpDelete("DeleteAllClients")]
    public ActionResult DeleteAllClients()
    {
        _clientService.DeleteAllClients();
        return Ok(new { message = "All clients have been deleted from the Database" });
    }

    [HttpGet("GetClientByEmail")]
    public ActionResult<Client?> GetClientByEmail([FromQuery] string email)
    {
        return _clientService.GetClientByEmail(email);
    }

    /*
    // EDIT THE BALANCE OF A CLIENT
    [HttpPost("EditBalance")]
    public ActionResult EditBalance([FromBody] System.Text.Json.JsonElement body) // PARAMETER DEFINED TO TAKE A JSON FROM POSTMAN
    {
        if (body.ValueKind != System.Text.Json.JsonValueKind.Object)
            return BadRequest(new { message = "Invalid body" });

        if (!body.TryGetProperty("client", out var clientElem))
            return BadRequest(new { message = "Missing 'client' object in body" });

        if (!body.TryGetProperty("amount", out var amountElem))
            return BadRequest(new { message = "Missing 'amount' in body" });

        Api.Models.Client? client;
        try
        {
            client = System.Text.Json.JsonSerializer.Deserialize<Api.Models.Client>(clientElem.GetRawText());
        }
        catch
        {
            return BadRequest(new { message = "Invalid 'client' object" });
        }

        if (client == null)
            return BadRequest(new { message = "Invalid 'client' object" });

        decimal amount;
        try
        {
            amount = amountElem.GetDecimal();
        }
        catch
        {
            // fallback to double -> decimal
            try { amount = Convert.ToDecimal(amountElem.GetDouble()); }
            catch { return BadRequest(new { message = "Invalid 'amount' value" }); }
        }

        _clientService.EditBalance(client, amount);
        return Ok(new { message = "Client's balance has been modified" });
    }
    */
}
