using Api.Models;

namespace Api.Repositories;

public interface ITransactionRepository
{
    List<Transaction> GetAllTransactions();
    void InsertTransaction(Transaction transaction);
    void DeleteTransaction(Transaction transaction);
    void DeleteAllTransactions();
}
