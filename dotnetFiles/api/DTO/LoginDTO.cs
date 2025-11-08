namespace Api.DTO;


public class LoginDTO
{
    public int LoginId{ get; set; }
    public string Email { get; set; } = "";

    public string Password { get; set; } = "";
}