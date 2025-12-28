namespace Api.DTO;

// DTO to manage bank transaction to the casino
public class WithdrawDTO
{
    // VidaLoca account to add money
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    
    // Iban of the BankuumTubo account to subtract the money
    public string? BankIban { get; set; }
}
