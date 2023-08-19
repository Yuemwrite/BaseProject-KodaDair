using BaseProject.Application.Handlers.Users.Queries;
using BaseProject.IdentityAPI.Controllers.BaseController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.IdentityAPI.Controllers;

public class LoginsController : BaseApiController
{
    /// <summary>Sisteme giriş yap</summary>
    /// <response code="200"></response>
    [AllowAnonymous]
    [HttpGet("login")]
    public async Task<ActionResult> Login(string username, string password)
    {
        return Ok(await Mediator.Send(new GetUserLoginQuery(){UserName = username, Password = password}));
    }
    
    /// <summary>Refresh Token</summary>
    /// <response code="200"></response>
    [AllowAnonymous]
    [HttpGet("refreshToken")]
    public async Task<ActionResult> RefreshToken(string refreshToken)
    {
        return Ok(await Mediator.Send(new RefreshTokenLoginCommand() { RefreshToken = refreshToken }));
    }
    
}