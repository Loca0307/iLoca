namespace Api.DTO;


public class LoginDTO
{
    public int LoginId{ get; set; }
    public string Email { get; set; } = "";
    public string Username { get; set; } = "";
    public int ClientId { get; set; }
}