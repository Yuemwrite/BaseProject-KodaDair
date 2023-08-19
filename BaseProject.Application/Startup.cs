using System.Reflection;
using BaseProject.Application.Abstraction.Abstract;
using BaseProject.Application.Identity.Tokens;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BaseProject.Application;

public static class Startup
{
    public static void AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services
            .AddValidatorsFromAssembly(assembly)
            .AddMediatR(assembly);
    }
}