using Api.Repositories;
using System.Collections.Generic;
using Api.Models;


namespace Api.Services;


// Interface to be implemented by the CLientService class
public interface IClientService
{

    public List<Client> GetAllClients();

    public void InsertClient(Client client);

    public void DeleteClient(Client client);

}