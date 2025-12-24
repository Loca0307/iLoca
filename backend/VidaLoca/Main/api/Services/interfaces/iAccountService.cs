using Api.Models;

namespace Api.Services;

public interface IAccountService{
    List<Account> GetAllAccounts();
}