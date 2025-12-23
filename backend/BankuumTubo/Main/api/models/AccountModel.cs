namespace Api.Models;


public class Account
{   
    public int AccountId{ get; set; }
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string Username { get; set; } = "";
    public int ClientId { get; set; }

}