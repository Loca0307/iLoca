using Api.Models;
using Api.DTO;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Http.HttpResults;

using System.Linq;

namespace Api.Controllers;

[ApiController]
[Route("/[controller]")]
[EnableCors("VidaLocaCors")] // To limit VidaLoca calls only to these controllers
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }


    // RETURN ALL THE ACCOUNTS
    [HttpGet("ShowAccounts")]
    public ActionResult<List<AccountDTO>> GetAllAccounts()
    {
        var accounts = _accountService.GetAllAccounts();

        // Here get given what the controller will return to the frontend
        var accountDTOs = accounts.Select(a => new AccountDTO // Select is like map in java and js
        {
            AccountId = a.AccountId,
            Email = a.Email,
            Username = a.Username,
        });

        return Ok(accountDTOs);
    }



    [HttpGet("GetUsernameByEmail")]
    public ActionResult<string> GetUsernameByEmail([FromQuery] string email)
    {
        var username = _accountService.GetUsernameByEmail(email);
        if (username == null) return NotFound();
        return Ok(username);
    }


    // ADD ACCOUNT TO THE DATABASE
    [HttpPost("InsertAccount")]
    public ActionResult<AccountDTO> InsertAccount([FromBody] Account account)
    {

        _accountService.InsertAccount(account);

        var accountDTO = new AccountDTO
        {
            AccountId = account.AccountId,
            Email = account.Email,
            Username = account.Username,
        };

        return Ok(accountDTO);
    }



    [HttpDelete("DeleteAccount")]
    public ActionResult DeleteAccount(Account account)
    {
        _accountService.DeleteAccount(account);
        return Ok(new { message = "Account has been succesfully removed" });
    }


    // AUTHENTICATE ACCOUNT ATTEMPT
    [HttpPost("Authenticate")]
    public ActionResult<AccountDTO> Authenticate(Account account)
    {
        var isValid = _accountService.Authenticate(account.Email, account.Password);
        if (!isValid)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }

        var accountDTO = new AccountDTO
        {
            AccountId = account.AccountId,
            Email = account.Email,
            Username = account.Username,
        };
        return Ok(accountDTO);
    }

    [HttpDelete("DeleteAllAccounts")]
    public ActionResult DeleteAllAccounts()
    {
        _accountService.DeleteAllAccounts();

        return Ok(new { message = "All accounts have been deleted from the Database" });
    }

    [HttpPost("Withdraw")]
    public ActionResult WithDraw([FromBody] WithdrawDTO dto)
    {
        if (dto == null) return BadRequest();

        bool success = _accountService.WithDraw(dto.AccountId, dto.Amount);

        if (!success)
        {
            return BadRequest(new { message = "Withdraw failed (insufficient funds or invalid request)" });
        }

        return Ok(new { message = "Withdraw successful" });
    }

    [HttpGet("GetBalanceByAccount")]
    public ActionResult<decimal> GetBalanceByAccount([FromQuery] int accountId)
    {
        var balance = _accountService.GetBalance(accountId);
        if (balance == null) return NotFound();
        return Ok(balance.Value);
    }
}