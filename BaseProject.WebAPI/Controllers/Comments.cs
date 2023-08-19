
using BaseProject.Application.Handlers.Comments.Commands;
using BaseProject.Application.Handlers.Comments.Queries;
using BaseProject.Application.Handlers.Users.Commands;
using BaseProject.WebAPI.Controllers.BaseController;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.WebAPI.Controllers;

public class Comments : BaseApiController
{
    /// <summary>Gönderiye Yorum Yap</summary>
    /// <response code="200"></response>
    [HttpPost("create")]
    public async Task<IActionResult> Add([FromBody] CreateCommentCommand createCommentCommand)
    {
        return Created("", await Mediator.Send(createCommentCommand));
    }
    
    /// <summary>Gönderiye Ait Yorumları Listele</summary>
    /// <response code="200"></response>
    [HttpPost("list")]
    public async Task<IActionResult> List([FromBody] GetCommentsQuery getCommentsQuery)
    {
        return Created("", await Mediator.Send(getCommentsQuery));
    }
    
    /// <summary>Yorumu Güncelle</summary>
    /// <response code="200"></response>
    [HttpPut("update")]
    public async Task<IActionResult> Add([FromBody] UpdateCommentCommand updateCommentCommand)
    {
        return Created("", await Mediator.Send(updateCommentCommand));
    }
}