using BaseProject.Application.Common.ServiceLifeTime;
using Microsoft.AspNetCore.Http;

namespace BaseProject.Application.Common.Interfaces;

public interface IHttpContextAccessorService : ITransientService
{
    HttpContext GetHttpContextAccessor();
}