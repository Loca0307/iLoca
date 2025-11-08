using Api.Models;
using Api.Repositories;
using BCrypt.Net;

namespace Api.Services;


public class LoginService : ILoginService
{

    public readonly ILoginRepository _loginRepository;

    public LoginService(ILoginRepository loginRepository)
    {
        _loginRepository = loginRepository;
    }
    
    public List<Login> GetAllLogins()
    {
       return _loginRepository.GetAllLogins();
    }

    public void InsertLogin(Login login)
    {
        // Hash the password before saving to database
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(login.Password);
        
        // Create a new login object with hashed password
        var loginToSave = new Login
        {
            LoginId = login.LoginId,
            Email = login.Email,
            Password = hashedPassword
        };
        
        _loginRepository.InsertLogin(loginToSave);
    }

    public void DeleteLogin(Login login)
    {
        _loginRepository.DeleteLogin(login);
    }


    // Method to autenticate a login attempt
    public bool Authenticate(string email, string password)
    {
        // Get the Login by email from the repository
        var Login = _loginRepository.GetLoginByEmail(email);
        if (Login == null)
            return false; // Login not found

        // Verify the password using BCrypt
        bool isValid = BCrypt.Net.BCrypt.Verify(password, Login.Password);
        return isValid;
    }
    
}