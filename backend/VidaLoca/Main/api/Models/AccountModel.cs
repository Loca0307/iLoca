namespace Api.Models;

public class Account
{
    public int AccountId{ get; set; }
    public string Email { get; set; } = ""; // Current way to access bank account
    public string Password { get; set; } = "";
    public string Username { get; set; } = "";

}
