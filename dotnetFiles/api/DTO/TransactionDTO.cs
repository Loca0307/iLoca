namespace Api.DTO;

public class TransactionDTO{
    
    public int TransactionId { get; set; }
    public string Sender { get; set; } = "";
    public int Receiver { get; set; }
    public int Amount { get; set; } = 0;
    public DateTime DateTime { get; set; }
    public string Reason { get; set; } = "";

}