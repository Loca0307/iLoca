using Api.Models;

namespace Api.Services;

public interface IAccountService{
    public List<Account> GetAllAccounts();
    public void InsertAccount(Account account);

}