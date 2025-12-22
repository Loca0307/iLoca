using Api.Models;
using Api.Repositories;
using BCrypt.Net;

namespace Api.Services;


public class LoginService : ILoginService
{

    public readonly ILoginRepository _loginRepository;
    public readonly IClientRepository _clientRepository;

    public LoginService(ILoginRepository loginRepository, IClientRepository clientRepository)
    {
        _loginRepository = loginRepository;
        _clientRepository = clientRepository;
    }
    
    public List<Login> GetAllLogins()
    {
       return _loginRepository.GetAllLogins();
    }

    public void InsertLogin(Login login)
    {
        if (_loginRepository.GetLoginByEmail(login.Email) != null)
        {
            throw new InvalidOperationException("a Account with this email already exists. Did you forget your password?");
        }
        var clientCheck = _clientRepository.GetClientByEmail(login.Email);
        if (clientCheck == null)
        {
            // Do not allow creating a Login without an associated Client
            throw new InvalidOperationException("A client with the provided email must exist before creating a login.");
        }
        // Hash the password before saving to database
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(login.Password);
        
        // Create a new login object with hashed password
        var loginToSave = new Login
        {
            LoginId = login.LoginId,
            Email = login.Email,
            Password = hashedPassword,
            Username = login.Username,
            ClientId = clientCheck.ClientId
        };
        
        _loginRepository.InsertLogin(loginToSave);
    }

    public void DeleteLogin(Login login)
    {
        _loginRepository.DeleteLogin(login);
    }

    public void DeleteAllLogins()
    {
        _loginRepository.DeleteAllLogins();
    }

    // Method to autenticate a login attempt
    public bool Authenticate(string email, string password)
    {
        // Get the Login by email from the repository
        var Login = _loginRepository.GetLoginByEmail(email);
        if (Login == null)
            return false; // Login not found

        // Check for null or empty password hash
        if (string.IsNullOrEmpty(Login.Password))
            return false; // Invalid stored hash

        // Verify the password using BCrypt
        bool isValid = BCrypt.Net.BCrypt.Verify(password, Login.Password);
        return isValid;
    }



    
}