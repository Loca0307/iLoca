using Api.Models;
using System.Collections.Generic;

namespace Api.Repositories;

public interface IClientRepository
{
    public List<Client> GetAllClients();
    public void InsertClient(Client client);
    public void DeleteClient(Client client);
    public void DeleteAllClients();
    public Client? GetClientByEmail(string email);
    public Client? GetClientByIban(string iban);
}