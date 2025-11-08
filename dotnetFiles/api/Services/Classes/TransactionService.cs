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
    /*
    public void InsertTransaction(Transaction transaction)
    {
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
    */
}