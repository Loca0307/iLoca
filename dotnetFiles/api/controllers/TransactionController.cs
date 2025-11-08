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

    public TransactionController(ITransactionService transactionService)
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
            Sender = t.Sender,
            Receiver = t.Receiver,
            Amount = t.Amount,
            DateTime = t.DateTime,
            Reason = t.Reason
        }).ToList();

        return Ok(transactionDTOs);
    }
}