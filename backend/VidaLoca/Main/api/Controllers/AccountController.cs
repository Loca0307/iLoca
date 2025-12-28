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

    [HttpGet("GetAccountByEmail")]
    public ActionResult<AccountDTO> GetAccountByEmail([FromQuery] string email)
    {
        if (string.IsNullOrEmpty(email)) return BadRequest();
        var acct = _accountService.GetAccountByEmail(email);
        if (acct == null) return NotFound();
        var dto = new AccountDTO
        {
            AccountId = acct.AccountId,
            Email = acct.Email,
            Username = acct.Username,
        };
        return Ok(dto);
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

        // Fetch the account from DB to return canonical data (accountId, username)
        var acct = _accountService.GetAccountByEmail(account.Email);
        if (acct == null) return NotFound();

        var accountDTO = new AccountDTO
        {
            AccountId = acct.AccountId,
            Email = acct.Email,
            Username = acct.Username,
        };

        return Ok(accountDTO);
    }

    [HttpDelete("DeleteAllAccounts")]
    public ActionResult DeleteAllAccounts()
    {
        _accountService.DeleteAllAccounts();

        return Ok(new { message = "All accounts have been deleted from the Database" });
    }



    [HttpGet("GetBalanceByAccount")]
    public ActionResult<decimal> GetBalanceByAccount([FromQuery] int accountId)
    {
        var balance = _accountService.GetBalance(accountId);
        if (balance == null) return NotFound();
        return Ok(balance.Value);
    }

    [HttpPost("WithdrawFromBank")]
    public ActionResult WithdrawFromBank([FromBody] WithdrawDTO dto)
    {
        if (dto == null || string.IsNullOrEmpty(dto.BankIban)) return BadRequest();

        var targetAccountId = dto.AccountId;

        // If caller didn't provide an accountId, resolve it from the bank IBAN -> BankuumTubo client email -> VidaLoca account
        if (targetAccountId == 0)
        {
            var bankClient = _accountService.GetBankClientByIban(dto.BankIban);
            if (bankClient == null || string.IsNullOrEmpty(bankClient.Email))
            {
                return BadRequest(new { message = "Unable to resolve bank client by IBAN" });
            }

            var vidaAccount = _accountService.GetAccountByEmail(bankClient.Email);
            if (vidaAccount == null)
            {
                return BadRequest(new { message = "No VidaLoca account found for bank client's email" });
            }

            targetAccountId = vidaAccount.AccountId;
            // If the bank client doesn't have enough funds, fail fast with informative message
            if (bankClient.Balance < dto.Amount)
            {
                return BadRequest(new { message = "Bank account has insufficient funds" });
            }
        }

        var success = _accountService.TransferFromBankToVida(targetAccountId, dto.BankIban, dto.Amount);
        if (!success) return BadRequest(new { message = "Transfer failed (insufficient funds or invalid request)" });
        return Ok(new { message = "Transfer successful", accountId = targetAccountId });
    }

    [HttpGet("VerifyBankIban")]
    public ActionResult<BankClientInfo> VerifyBankIban([FromQuery] string iban)
    {
        if (string.IsNullOrEmpty(iban)) return BadRequest();
        var info = _accountService.GetBankClientByIban(iban);
        if (info == null) return NotFound();
        return Ok(info);
    }

    [HttpPost("UpdateBetMoney")]
    public ActionResult UpdateBetMoney([FromBody] UpdateBetDTO dto)
    {
        if (dto == null || dto.Amount <= 0 || string.IsNullOrEmpty(dto.Email)) return BadRequest();

        var success = _accountService.UpdateBetMoney(dto.Amount, dto.Operation, dto.Email);
        if (!success) return BadRequest(new { message = "Account not found or update failed" });

        return Ok(new { message = "Balance updated" });
    }
}