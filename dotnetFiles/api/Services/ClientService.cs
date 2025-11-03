using Api.Models;
using Api.Repositories;
using System.Collections.Generic;

namespace Api.Services;

public class ClientService
{
    private readonly ClientRepository _clientRepository;

    // Methods to use the repository methods conbined with logic here
    public ClientService(ClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public List<Client> GetAllClients()
    {
        return _clientRepository.GetAllClients();
    }

    public void AddClient(Client client)
    {
        _clientRepository.InsertClient(client);
    }
}
