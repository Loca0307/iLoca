
namespace Api.DTO;


// This files defines what we want to send to the frontend
// this way we don't show sensible or useless informations
public class ClientDTO
{
    public int ClientId { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
}