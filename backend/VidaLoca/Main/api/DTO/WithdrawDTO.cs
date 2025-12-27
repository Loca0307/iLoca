namespace Api.DTO;

// DTO to manage bank transaction to the casino
public class WithdrawDTO
{
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
}
