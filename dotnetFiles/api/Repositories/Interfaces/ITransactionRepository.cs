using Api.Models;

namespace Api.Repositories;

public interface ITransactionRepository
{
    List<Transaction> GetAllTransactions();
    void DeleteTransaction(Transaction transaction);
    void DeleteAllTransactions();
    void InsertTransaction(Transaction transaction);
}
