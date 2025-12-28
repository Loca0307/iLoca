using Api.Models;
using Api.Repositories;
using BCrypt.Net;

namespace Api.Services;


public class AccountService : IAccountService
{

    public readonly IAccountRepository _accountRepository;
    public readonly IClientRepository _clientRepository;

    public AccountService(IAccountRepository accountRepository, IClientRepository clientRepository)
    {
        _accountRepository = accountRepository;
        _clientRepository = clientRepository;
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
        var clientCheck = _clientRepository.GetClientByEmail(account.Email);
        if (clientCheck == null)
        {
            // Do not allow creating a Account without an associated Client
            throw new InvalidOperationException("A client with the provided email must exist before creating a Account.");
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
            ClientId = clientCheck.ClientId
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


    public string? GetUsernameByEmail(string email)
    {
        return _accountRepository.GetUsernameByEmail(email);
    }



    
}