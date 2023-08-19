using BaseProject.Application.Handlers.Comments.Commands;
using BaseProject.Application.Handlers.Replies.Commands;
using BaseProject.Application.Handlers.Replies.Queries;
using BaseProject.WebAPI.Controllers.BaseController;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.WebAPI.Controllers;

public class RepliesController : BaseApiController
{
    /// <summary>Yoruma Yanıt Ekle</summary>
    /// <response code="200"></response>
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateReplyCommand createReplyCommand)
    {
        return Created("", await Mediator.Send(createReplyCommand));
    }
    
    /// <summary>Yanıtı Güncelle</summary>
    /// <response code="200"></response>
    [HttpPut("update")]
    public async Task<IActionResult> UpdateExperience([FromBody] UpdateReplyCommand updateReplyCommand)
    {
        return Created("", await Mediator.Send(updateReplyCommand));
    }
    
    /// <summary>Yoruma Yapılan Yanıtları Listele</summary>
    /// <response code="200"></response>
    [HttpPost("list")]
    public async Task<IActionResult> List([FromBody] GetRepliesQuery getRepliesQuery)
    {
        return Created("", await Mediator.Send(getRepliesQuery));
    }
    
    /// <summary>Yanıtı Sil</summary>
    /// <response code="200"></response>
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteReplyCommand deleteReplyCommand)
    {
        return Created("", await Mediator.Send(deleteReplyCommand));
    }
}