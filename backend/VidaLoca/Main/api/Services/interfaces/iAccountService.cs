using Api.Models;
using Api.DTO;

namespace Api.Services;

public interface IAccountService
{
    public List<Account> GetAllAccounts();
    public void InsertAccount(Account account);
    public void DeleteAccount(Account account);
    public bool Authenticate(string email, string password);
    // Return the Account model for a given email (or null)
    public Account? GetAccountByEmail(string email);

    // Return username for a given email, or null if not found
    public string? GetUsernameByEmail(string email);

    public void DeleteAllAccounts();

    public bool WithDraw(int accountId, decimal amount);
    public decimal? GetBalance(int accountId);
    public bool TransferFromBankToVida(int accountId, string bankIban, decimal amount);
    public BankClientInfo? GetBankClientByIban(string iban);
}