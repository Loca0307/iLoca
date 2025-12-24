using Api.Models;

namespace Api.Repositories;

public interface IAccountRepository
{
    List<Account> GetAllAccounts();

}
