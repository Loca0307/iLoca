using Api.Models;


namespace Api.Repositories;

public interface IAccountRepository
{
    public List<Account> GetAllAccounts();
    public void InsertAccount(Account account);

    public void DeleteAccount(Account account);

    public Account? GetAccountByEmail(string email);
    public string? GetUsernameByEmail(string email);

    public void DeleteAllAccounts();

    public bool WithDraw(int accountId, decimal amount);
    public decimal? GetBalance(int accountId);
}