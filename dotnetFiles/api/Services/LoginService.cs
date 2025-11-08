using Api.Models;
using Api.Repositories;

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
        _loginRepository.InsertLogin(login);
    }

    public void DeleteLogin(Login login)
    {
        _loginRepository.DeleteLogin(login);
    }
}