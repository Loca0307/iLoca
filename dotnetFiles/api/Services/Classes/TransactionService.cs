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