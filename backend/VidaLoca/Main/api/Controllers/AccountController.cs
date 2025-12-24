using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Api.Models;
using Api.DTO;
using Api.Services;
using System.Collections.Generic;
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

    [HttpGet("ShowAccounts")]
    public ActionResult<List<AccountDTO>> GetAllAccounts()
    {
        var accounts = _accountService.GetAllAccounts();

        var accountDTOs = accounts.Select(a => new AccountDTO
        {
            AccountId = a.AccountId,
            Email = a.Email,
            Username = a.Username,
            ClientId = a.ClientId
        });

        return Ok(accountDTOs);

    }

    [HttpGet("InsertAccount")]
    public ActionResult<AccountDTO> InsertAccount(Account account)
    {
        _accountService.InsertAccount(account);

        var accountDTO = new AccountDTO
        {
            AccountId = account.AccountId,
            Email = account.Email,
            Username = account.Username,
            ClientId = account.ClientId
        };

        return accountDTO;
    }

}