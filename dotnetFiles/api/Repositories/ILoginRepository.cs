using Api.Models;


namespace Api.Repositories;

public interface ILoginRepository
{
    public List<Login> GetAllLogins();
    public void InsertLogin(Login login);
}