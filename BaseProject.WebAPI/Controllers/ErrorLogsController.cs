using BaseProject.Application.Handlers.ErrorLogs.Queries;
using BaseProject.Application.Handlers.MongoDb;
using BaseProject.WebAPI.Controllers.BaseController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.WebAPI.Controllers;

public class ErrorLogsController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet("list")]
    public async Task<ActionResult> List()
    {
        return Ok(await Mediator.Send(new GetErrorLogsQuery()));
    }
}