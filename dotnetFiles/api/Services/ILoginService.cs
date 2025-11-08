using Api.Models;

namespace Api.Services;

public interface ILoginService
{
    public List<Login> GetAllLogins();
    public void InsertLogin(Login login);

}