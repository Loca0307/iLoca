namespace Api.Models;

public class Account
{
    public int ClientId { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = ""; // Current way to access bank account
    public string Password {get; set;} = "";

}
