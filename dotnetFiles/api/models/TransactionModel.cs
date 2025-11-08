namespace Api.Models;

public class Transaction{
    
    public int TransactionId { get; set; }
    public string Sender { get; set; } = "";
    public string Receiver { get; set; } = "";
    public int Amount { get; set; } = 0;
    public DateTime DateTime { get; set; }
    public string Reason { get; set; } = "";

}