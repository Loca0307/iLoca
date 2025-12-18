using Api.Models;

namespace Api.Services;

public interface ILoginService
{
    public List<Login> GetAllLogins();
    public void InsertLogin(Login login);
    public void DeleteLogin(Login login);
    public bool Authenticate(string email, string password);

    public void DeleteAllLogins();
}