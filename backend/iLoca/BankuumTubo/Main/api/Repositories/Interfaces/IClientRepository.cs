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
    public Boolean CheckBalance(Client client, decimal amount);
    void EditBalance(Client client, decimal amount);
    
    // Perform an atomic transfer between two clients and record the transaction
    // This method should ensure both balance updates and recording the transaction
    // happen inside a single DB transaction with proper row locking.
    void TransferAndRecordTransaction(Client Sender, Client Receiver, Transaction transaction);

}