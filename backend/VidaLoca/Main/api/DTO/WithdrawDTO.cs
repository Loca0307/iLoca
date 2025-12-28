namespace Api.DTO;

// DTO to manage bank transaction to the casino
public class WithdrawDTO
{
    // VidaLoca account to add money
    public int AccountId { get; set; }
    public decimal Amount { get; set; }

    // Iban of the BankuumTubo account to subtract the money
    public string? BankIban { get; set; }
    // If true, perform a deposit from VidaLoca to BankuumTubo (Vida -> Bank). Default false = bank -> vida
    public bool IsDeposit { get; set; } = false;
}
