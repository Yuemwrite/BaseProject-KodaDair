using System.Reflection;
using BaseProject.Application.Common.ServiceLifeTime;
using Microsoft.Extensions.DependencyInjection;

namespace BaseProject.Application.Common;

public static class Startup
{
    public static IServiceCollection AddServices(this IServiceCollection services) =>
        services.AddServices(typeof(ITransientService), ServiceLifetime.Transient);

    private static IServiceCollection AddServices(this IServiceCollection services, Type interfaceType, ServiceLifetime lifetime)
    {
        var interfaceTypes =
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => interfaceType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Service = t.GetInterfaces().FirstOrDefault(),
                    Implementation = t
                })
                .Where(t => t.Service is not null && interfaceType.IsAssignableFrom(t.Service));
        

        foreach (var type in interfaceTypes)
            services.AddService(type.Service!, type.Implementation, lifetime);
        

        return services;
    }
    
    private static IServiceCollection AddService(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime) =>
        lifetime switch
        {
            ServiceLifetime.Transient => services.AddTransient(serviceType, implementationType),
            ServiceLifetime.Scoped => services.AddScoped(serviceType, implementationType),
            ServiceLifetime.Singleton => services.AddSingleton(serviceType, implementationType),
            _ => throw new ArgumentException("Invalid lifeTime", nameof(lifetime))
        };

    
    
}