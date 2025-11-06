using Api.Models;
using System.Collections.Generic;

namespace Api.Repositories;

public interface IClientRepository
{
    List<Client> GetAllClients();
    void InsertClient(Client client);
    void DeleteClient(Client client);
}