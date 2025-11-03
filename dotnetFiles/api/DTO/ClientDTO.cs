// Api/DTOs/ClientDTO.cs
namespace Api.DTOs;


// This files defines what we want to send to the frontend
// this way we don't show sensible or useless informations

// NOT IN USE FOR NOW!!!
public class ClientDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
