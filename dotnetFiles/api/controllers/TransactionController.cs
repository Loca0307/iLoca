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

    public TransactionController(ITransactionService transactionService, ILoginService loginService)
    {
        _transactionService = transactionService;
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
            SenderEmail = t.SenderEmail,
            ReceiverIban = t.ReceiverIban,
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

        _transactionService.InsertTransaction(transaction);

        var transactionDTO = new TransactionDTO
        {
            TransactionId = transaction.TransactionId,
            SenderEmail = transaction.SenderEmail,
            ReceiverIban = transaction.ReceiverIban,
            Amount = transaction.Amount,
            DateTime = transaction.DateTime,
            Reason = transaction.Reason ?? string.Empty,

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