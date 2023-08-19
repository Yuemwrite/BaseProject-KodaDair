
using BaseProject.Application.Handlers.Sharings.Commands;
using BaseProject.Application.Handlers.Sharings.Queries;
using BaseProject.Application.Handlers.Users.Commands;
using BaseProject.WebAPI.Controllers.BaseController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.WebAPI.Controllers;

public class QuestionSharings : BaseApiController
{
    /// <summary>Gönderi Oluştur</summary>
    /// <response code="200"></response>
    [HttpPost("questionsharings/create")]
    public async Task<IActionResult> Add([FromBody] CreateSharingCommand createSharing)
    {
        return Created("", await Mediator.Send(createSharing));
    }
    
    /// <summary>Gönderileri Listele</summary>
    /// <response code="200"></response>
    [HttpPost("questionsharings/list")]
    public async Task<IActionResult> Get([FromBody] GetQuestionSharingsQuery createSharing)
    {
        return Created("", await Mediator.Send(createSharing));
    }
    
    /// <summary>Takip Edilen Kullanıcılara Ait Gönderileri Listele</summary>
    /// <response code="200"></response>
    [HttpGet("follow/sharings/list")]
    public async Task<ActionResult> GetFollowSharings()
    {
        return Ok(await Mediator.Send(new GetFollowersSharingsQuery()));
    }

    /// <summary>Gönderi Sil</summary>
    /// <response code="200"></response>
    [HttpDelete("questionsharings/delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteSharingCommand deleteSharingCommand)
    {
        return Created("", await Mediator.Send(deleteSharingCommand));
    }
    
    /// <summary>Gönderiyi Kaydet / Favoriye Ekle</summary>
    /// <response code="200"></response>
    [HttpPost("savedPost/create")]
    public async Task<IActionResult> SavedPost([FromBody] UpdateSavedSharingCommand updateSavedSharingCommand)
    {
        return Created("", await Mediator.Send(updateSavedSharingCommand));
    }
    
    /// <summary>Favoriye Eklenen Gönderileri Listele</summary>
    /// <response code="200"></response>
    [HttpPost("savedpost/list")]
    public async Task<IActionResult> SavedPost([FromBody] GetSavedSharingsQuery getSavedSharingsQuery)
    {
        return Created("", await Mediator.Send(getSavedSharingsQuery));
    }
    
    /// <summary>Gönderiyi Sabitle</summary>
    /// <response code="200"></response>
    [HttpPut("isfixed")]
    public async Task<IActionResult> Add([FromBody] UpdateSharingFixedCommand updateSharingFixedCommand)
    {
        return Created("", await Mediator.Send(updateSharingFixedCommand));
    }
    
}