using Api.Models;

namespace Api.Services;


public interface ITransactionService
{
    public List<Transaction> GetAllTransactions();
    public void InsertTransaction(Transaction transaction);
    public void DeleteTransaction(Transaction transaction);
    public void DeleteAllTransactions();

}
