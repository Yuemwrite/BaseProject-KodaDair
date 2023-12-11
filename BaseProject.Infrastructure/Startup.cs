using System.Reflection;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.BackgroundServices;
using BaseProject.Application.Common;
using BaseProject.Application.Identity.Tokens;
using BaseProject.Infrastructure.Auth;
using BaseProject.Infrastructure.Context;


using BaseProject.Infrastructure.Middleware;
using BaseProject.Infrastructure.Persistence;
using BaseProject.Infrastructure.Persistence.Base;
using BaseProject.Infrastructure.Persistence.Initialization;
using BaseProject.Infrastructure.Quartz;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BaseProject.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        return services
            .AddPersistence(config)
            .AddDbContextBehavior<DbContextBehaivor>()
            .AddTransientService()
            .AddSingletonService()
            .AddAuth(config)
            .AddMemoryCache()
            .RegisterDatabase(config)
            .AddExceptionMiddleware()
            .AddQuartz()
            .AutoMapper();
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder) =>
        builder
            .UseCurrentUser()
            .UseExceptionMiddleware();
    
    public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        // Create a new scope to retrieve scoped services
        using var scope = services.CreateScope();

        scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
            .InitializeDatabases();
    }


}