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
        var SenderEmail = transaction.SenderEmail;

        // 1)
        var receiverIban = transaction.ReceiverIban;

        // Lookup receiver and sender using clientRepository
        var sender = _clientRepository.GetClientByEmail(SenderEmail);
        var receiver = _clientRepository.GetClientByIban(receiverIban);
        
        if (receiver == null)
        {
            throw new InvalidOperationException("Receiver with provided IBAN not found.");
        }
        
        // 2) 


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