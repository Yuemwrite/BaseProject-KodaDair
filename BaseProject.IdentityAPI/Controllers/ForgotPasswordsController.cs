using BaseProject.Application.Handlers.Users.Commands;
using BaseProject.IdentityAPI.Controllers.BaseController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.IdentityAPI.Controllers;

public class ForgotPasswordsController : BaseApiController
{
    /// <summary>Şifremi unuttum akışı başlat</summary>
    /// <response code="201"></response>
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> ForgotPassword(
        [FromBody] CreateForgotPasswordTransactionCommand createForgotPasswordTransactionCommand)
    {
        return Created("", await Mediator.Send(createForgotPasswordTransactionCommand));
    }

    /// <summary>Şifremi unuttum akışında, tek kullanımlık şifre ile yeni şifre belirle</summary>
    /// <response code="201"></response>
    [AllowAnonymous]
    [HttpPost("approval")]
    public async Task<IActionResult> ForgotPasswordApproval(
        [FromBody] CreateForgotPasswordTransactionApprovalCommand createForgotPasswordTransactionApprovalCommand)
    {
        return Created("", await Mediator.Send(createForgotPasswordTransactionApprovalCommand));
    }
}