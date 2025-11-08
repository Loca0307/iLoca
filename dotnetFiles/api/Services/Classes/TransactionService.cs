using Api.Models;
using Api.Repositories;

namespace Api.Services;

public class TransactionService : ITransactionService
{
    private readonly TransactionRepository _transactionRepository;

    public TransactionService(TransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public List<Transaction> GetAllTransactions()
    {
        return _transactionRepository.GetAllTransactions();
    }

    public void InsertTransaction(Transaction transaction)
    {
        var localTime = DateTime.Now;

        // To have the time taken in local time
        transaction.DateTime = DateTime.SpecifyKind(localTime, DateTimeKind.Local);
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