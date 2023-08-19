using BaseProject.Application.Handlers.Comments.Commands;
using BaseProject.Application.Handlers.Likes.Commands;
using BaseProject.Application.Handlers.Likes.Queries;
using BaseProject.WebAPI.Controllers.BaseController;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.WebAPI.Controllers;

public class LikesController : BaseApiController
{
    /// <summary>Gönderi/Yorum/Yanıt Beğen / Beğenmekten Vazgeç</summary>
    /// <response code="200"></response>
    [HttpPut]
    public async Task<IActionResult> Add([FromBody] CreateOrUpdateLikeCommand updateLikeCommand)
    {
        return Created("", await Mediator.Send(updateLikeCommand));
    }
    
    /// <summary>Gönderi/Yorum/Yanıt'a Ait Beğenileri Listele</summary>
    /// <response code="200"></response>
    [HttpPost("list")]
    public async Task<IActionResult> List([FromBody] GetLikesQuery getLikesQuery)
    {
        return Created("", await Mediator.Send(getLikesQuery));
    }
}