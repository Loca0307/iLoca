namespace Api.DTO;

public class BankClientInfo
{
    public int ClientId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Iban { get; set; } = string.Empty;
}
