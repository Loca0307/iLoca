using Api.Models;

namespace Api.Repositories;

public interface IAccountRepository
{
    public List<Account> GetAllAccounts();
    public void InsertAccount(Account account);

}
