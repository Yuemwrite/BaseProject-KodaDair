using BaseProject.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BaseProject.Infrastructure.Common.Services;

public class HttpContextAccessorService : IHttpContextAccessorService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextAccessorService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public HttpContext GetHttpContextAccessor()
    {
        return _httpContextAccessor.HttpContext;
    }
}