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


    // RETURN ALL THE LOGINS(Only for Postman testing)
    [HttpGet("ShowLogins")]
    public ActionResult<List<LoginDTO>> GetAllLogins()
    {
        var logins = _loginService.GetAllLogins();

        var loginDTOs = logins.Select(l => new LoginDTO
        {
            LoginId = l.LoginId,
            Email = l.Email,
            Password = l.Password
        }
        );

        return Ok(loginDTOs);
    }



    // ADD LOGIN TO THE DATABASE
    [HttpPost("InsertLogin")]
    public ActionResult<LoginDTO> InsertLogin([FromBody] Login login)
    {
        _loginService.InsertLogin(login);

        var loginDTO = new LoginDTO
        {
            LoginId = login.LoginId,
            Email = login.Email,
            Password = login.Password
        };


        return Ok(loginDTO);
    }

    /*
    [HttpPost("Authenticate")]
    public ActionResult<> Authenticate(Login login)
    {
        
    }
    */
}