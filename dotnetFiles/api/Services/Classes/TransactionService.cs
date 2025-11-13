using Api.Models;
using Api.Repositories;

namespace Api.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IClientRepository _clientRepository;

    public TransactionService(ITransactionRepository transactionRepository,
    IClientRepository clientRepository, ILoginRepository loginRepository)
    {
        _transactionRepository = transactionRepository;
        _clientRepository = clientRepository;
    }

    public List<Transaction> GetAllTransactions()
    {
        return _transactionRepository.GetAllTransactions();
    }

    public void InsertTransaction(Transaction transaction)
    {
        // To have the time taken in local time
        var localTime = DateTime.Now;
        transaction.DateTime = DateTime.SpecifyKind(localTime, DateTimeKind.Local);

        /* 
           1) Check for existing Iban
           2) Check if logged in client has the sent amount 
           3) Update the sender and receiver clients' amounts
        */

        // Select clients
        var sender = _clientRepository.GetClientByEmail(transaction.SenderEmail);
        var receiver = _clientRepository.GetClientByIban(transaction.ReceiverIban);
        if (receiver == null)
        {
            throw new InvalidOperationException($"Receiver with IBAN '{transaction.ReceiverIban}' not found.");
        }

        if (sender == null)
        {
            throw new InvalidOperationException($"Sender with Email '{transaction.SenderEmail}' not found.");
        }

        // Check if the sender has sufficient funds
        var senderHasEnough = _clientRepository.CheckBalance(sender, transaction.Amount);
        if (!senderHasEnough)
        {
            throw new InvalidOperationException($"You don't have enough credit on your account for this transaction.");
        }


        // Edit the balance of both clients (subtract for the sender)

        _clientRepository.EditBalance(sender, transaction.Amount * -1);
        _clientRepository.EditBalance(receiver, transaction.Amount);

        _transactionRepository.InsertTransaction(transaction);
    }
    
    public void DeleteTransaction(Transaction transaction)
    {
        _transactionRepository.DeleteTransaction(transaction);
    }
    public void DeleteAllTransactions()
    {
        _transactionRepository.DeleteAllTransactions();
    }
}