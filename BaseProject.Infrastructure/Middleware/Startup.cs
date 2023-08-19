using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BaseProject.Infrastructure.Middleware;

public static class Startup
{
    public static IServiceCollection AddExceptionMiddleware(this IServiceCollection services) =>
        services.AddScoped<ExceptionHandlingMiddleware>();

    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app) =>
        app.UseMiddleware<ExceptionHandlingMiddleware>();
}