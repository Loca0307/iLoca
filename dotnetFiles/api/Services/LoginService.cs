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
    public void InsertLogin(Login login)
    {
        _loginRepository.InsertLogin(login);
    }
}