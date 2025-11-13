using Api.Models;
using Api.DTO;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Controllers;


[ApiController]
[Route("/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }


    // RETURN ALL THE LOGINS
    [HttpGet("ShowLogins")]
    public ActionResult<List<LoginDTO>> GetAllLogins()
    {
        var logins = _loginService.GetAllLogins();

        // Here get given what the controller will return to the frontend
        var loginDTOs = logins.Select(l => new LoginDTO
        {
            LoginId = l.LoginId,
            Email = l.Email,
            Username = l.Username,
            ClientId = l.ClientId
        }
        );

        return Ok(loginDTOs);
    }



    // ADD LOGIN TO THE DATABASE
    [HttpPost("InsertLogin")]
    public ActionResult<LoginDTO> InsertLogin([FromBody] Login login)
    {
        try
        {
            _loginService.InsertLogin(login);

            var loginDTO = new LoginDTO
            {
                LoginId = login.LoginId,
                Email = login.Email,
                Username = login.Username,
                ClientId = login.ClientId
            };

            return Ok(loginDTO);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    [HttpDelete("DeleteLogin")]
    public ActionResult DeleteLogin(Login login)
    {
        _loginService.DeleteLogin(login);
        return Ok(new { message = "Login has been succesfully removed" });
    }


    // AUTHENTICATE LOGIN ATTEMPT
    [HttpPost("Authenticate")]
    public ActionResult<LoginDTO> Authenticate(Login login)
    {
        var isValid = _loginService.Authenticate(login.Email, login.Password);
        if (!isValid)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }

        var loginDTO = new LoginDTO
        {
            LoginId = login.LoginId,
            Email = login.Email,
            Username = login.Username,
            ClientId = login.ClientId
        };
        return Ok(loginDTO);
    }

    [HttpDelete("DeleteAllLogins")]
    public ActionResult DeleteAllLogins()
    {
        _loginService.DeleteAllLogins();

        return Ok(new { message = "All logins have been deleted from the Database" });
    }
}