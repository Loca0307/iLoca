using Api.Models;
using Api.Repositories;

namespace Api.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public List<Account> GetAllAccounts()
    {
        return _accountRepository.GetAllAccounts();
    }

    public void InsertAccount(Account account)
    {
        _accountRepository.InsertAccount(account);
    }

}