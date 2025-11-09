using Api.DTO;
using Api.Services;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly ILoginService _loginService;

    public TransactionController(ITransactionService transactionService, ILoginService loginService)
    {
        _transactionService = transactionService;
        _loginService = loginService;
    }

    // GET ALL THE TRANSACTION
    [HttpGet("ShowTransactions")]
    public ActionResult<List<TransactionDTO>> GetAllTransactions()
    {
        var transactions = _transactionService.GetAllTransactions();

        // Here get given what the controller 
        // will return to the frontend as DTO
        var transactionDTOs = transactions.Select(t => new TransactionDTO
        {
            TransactionId = t.TransactionId,
            Sender = t.Sender ?? string.Empty,
            Receiver = t.Receiver ?? string.Empty,
            Amount = t.Amount,
            DateTime = t.DateTime,
            Reason = t.Reason ?? string.Empty
        }).ToList();

        return Ok(transactionDTOs);
    }

    // TO INSERT A TRANSACTION
    [HttpPost("InsertTransaction")]
    public ActionResult<TransactionDTO> InsertTransaction([FromBody] Transaction transaction)
    {
        // Server-side receiver existence check and capture matched login
        var receiver = (transaction.Receiver ?? string.Empty).Trim().ToLower();
        var logins = _loginService.GetAllLogins();
        var found = logins.FirstOrDefault(l => ((l.Username ?? string.Empty).ToLower() == receiver)
                                            || ((l.Email ?? string.Empty).ToLower() == receiver));

        if (found == null)
        {
            return BadRequest(new { message = "Receiver not found" });
        }

        // MODIFY THE BALANCES OF BOTH CLIENTS
        var sender = transaction.Sender;

        


        _transactionService.InsertTransaction(transaction);
        var transactionDTO = new TransactionDTO
        {
            TransactionId = transaction.TransactionId,
            Sender = transaction.Sender ?? string.Empty,
            Receiver = transaction.Receiver ?? string.Empty,
            Amount = transaction.Amount,
            DateTime = transaction.DateTime,
            Reason = transaction.Reason ?? string.Empty
        };

        return Ok(transactionDTO);
    }

    // TO DELETE A TRANSACTION
    [HttpDelete("DeleteTransaction")]
    public ActionResult DeleteTrasaction(Transaction transaction)
    {
        _transactionService.DeleteTransaction(transaction);
        return Ok(new { message = "The transaction has been deleted from the Database" });
    }

    // TO DELETE EVERY TRANSACTION
    [HttpDelete("DeleteAllTransactions")]
    public ActionResult DeleteAllTransactions(){
        _transactionService.DeleteAllTransactions();
        return Ok(new { message = "All the transactions has been deleted from the Database" });
    }
}