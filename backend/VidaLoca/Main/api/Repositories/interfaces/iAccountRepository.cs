using Api.Models;
using Api.DTO;


namespace Api.Repositories;

public interface IAccountRepository
{
    public List<Account> GetAllAccounts();
    public void InsertAccount(Account account);

    public void DeleteAccount(Account account);

    public Account? GetAccountByEmail(string email);
    public string? GetUsernameByEmail(string email);

    public void DeleteAllAccounts();

    public decimal? GetBalance(int accountId);
    public bool TransferFromBankToVida(int accountId, string bankIban, decimal amount);
    public BankClientInfo? GetBankClientByIban(string iban);

    public bool UpdateBetMoney(double amount, bool operation, string email);
}