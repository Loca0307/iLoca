using Api.Models;

namespace Api.Services;

public interface ILoginService
{
    public List<Login> GetAllLogins();
    public void InsertLogin(Login login);
    public void DeleteLogin(Login login);
    public bool Authenticate(string email, string password);

    // Return username for a given email, or null if not found
    public string? GetUsernameByEmail(string email);

    public void DeleteAllLogins();
}