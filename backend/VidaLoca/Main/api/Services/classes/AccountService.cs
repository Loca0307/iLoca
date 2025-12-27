using Api.Models;
using Api.DTO;
using Api.Repositories;
using BCrypt.Net;

namespace Api.Services;


public class AccountService : IAccountService
{

    public readonly IAccountRepository _accountRepository;

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
        if (_accountRepository.GetAccountByEmail(account.Email) != null)
        {
            throw new InvalidOperationException("a account with this email already exists. Did you forget your password?");
        }

        // Hash the password before saving to database
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(account.Password);
        
        // Create a new login object with hashed password
        var accountToSave = new Account
        {
            AccountId = account.AccountId,
            Email = account.Email,
            Password = hashedPassword,
            Username = account.Username,
        };
        
        _accountRepository.InsertAccount(accountToSave);
    }

    public void DeleteAccount(Account account)
    {
        _accountRepository.DeleteAccount(account);
    }

    public void DeleteAllAccounts()
    {
        _accountRepository.DeleteAllAccounts();
    }

    // Method to autenticate a Account attempt
    public bool Authenticate(string email, string password)
    {
        // Get the Account by email from the repository
        var account = _accountRepository.GetAccountByEmail(email);
        if (account == null)
            return false; // Account not found

        // Check for null or empty password hash
        if (string.IsNullOrEmpty(account.Password))
            return false; // Invalid stored hash

        // Verify the password using BCrypt
        bool isValid = BCrypt.Net.BCrypt.Verify(password, account.Password);
        return isValid;
    }

    public Account? GetAccountByEmail(string email)
    {
        return _accountRepository.GetAccountByEmail(email);
    }


    public string? GetUsernameByEmail(string email)
    {
        return _accountRepository.GetUsernameByEmail(email);
    }


    public bool WithDraw(int accountId, decimal amount)
    {
        if (amount <= 0) return false;
        var success = _accountRepository.WithDraw(accountId, amount);
        return success;
    }

    public decimal? GetBalance(int accountId)
    {
        return _accountRepository.GetBalance(accountId);
    }

    public bool TransferFromBankToVida(int accountId, string bankIban, decimal amount)
    {
        if (amount <= 0 || string.IsNullOrEmpty(bankIban)) return false;
        return _accountRepository.TransferFromBankToVida(accountId, bankIban, amount);
    }

    public BankClientInfo? GetBankClientByIban(string iban)
    {
        if (string.IsNullOrEmpty(iban)) return null;
        return _accountRepository.GetBankClientByIban(iban);
    }
    
}