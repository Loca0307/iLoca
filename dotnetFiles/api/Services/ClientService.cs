using Api.Models;
using Api.Repositories;
using System.Collections.Generic;

namespace Api.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    // Here it return a list of "Client" and not of
    // "ClientDTO" because the DTO are only for 
    // the frontend to be seen so in the controller
    public List<Client> GetAllClients()
    {   
        // For the "GET" request when you 
        // return something put keyword "return"
        return _clientRepository.GetAllClients();
    }

    public void InsertClient(Client client)
    {
        _clientRepository.InsertClient(client);
    }

    public void DeleteClient(Client client)
    {
        _clientRepository.DeleteClient(client);
    } 

    public void DeleteAllClients() {
        _clientRepository.DeleteAllClients();
    }

}
