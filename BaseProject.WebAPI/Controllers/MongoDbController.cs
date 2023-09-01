using BaseProject.Application.Handlers.MongoDb;
using BaseProject.Application.Handlers.Sharings.Commands;
using BaseProject.WebAPI.Controllers.BaseController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.WebAPI.Controllers;

public class MongoDbController : BaseApiController
{
    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<IActionResult> Add([FromBody] MongoDbCommand mongoDbCommand)
    {
        return Created("", await Mediator.Send(mongoDbCommand));
    }
    
    [AllowAnonymous]
    [HttpGet("list")]
    public async Task<ActionResult> List()
    {
        return Ok(await Mediator.Send(new GetReportQuery()));
    }
}