using System.Net.Mime;
using BaseProject.Application.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.WebAPI.Controllers.BaseController;

[Authorize]
[ApiController]
[Route("Api/[controller]")]
public class BaseApiController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    
    // [NonAction]
    // public virtual async Task<CreatedResult> Created<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    // {
    //     var result = await Mediator.Send(request, cancellationToken: cancellationToken);
    //
    //     return Created(string.Empty, result);
    // }
    //
    // [NonAction]
    // public virtual async Task<OkObjectResult> Ok<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    // {
    //     var result = await Mediator.Send(request, cancellationToken);
    //
    //     return Ok(result);
    // }

}