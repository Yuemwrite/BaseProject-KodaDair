using System.Reflection;
using AutoMapper;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Identity.Tokens;
using BaseProject.Application.MessageService;
using BaseProject.Application.MongoDb;
using BaseProject.Application.Redis;
using BaseProject.Infrastructure.Automapper;
using BaseProject.Infrastructure.Context;
using BaseProject.Infrastructure.Identity;
using BaseProject.Infrastructure.MessageService;
using BaseProject.Infrastructure.Persistence.Base;
using BaseProject.Infrastructure.Persistence.Initialization;
using BaseProject.Infrastructure.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EmailService = BaseProject.Infrastructure.Mailing.EmailService;

namespace BaseProject.Infrastructure.Persistence;

public static class Startup
{
    private static bool IsAssignableToGenericType(this Type givenType, Type genericType)
    {
        var givenTypeInfo = givenType.GetTypeInfo();

        if (givenTypeInfo.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            return true;

        foreach (var interfaceType in givenType.GetInterfaces())
        {
            if (interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType)
                return true;
        }

        if (givenTypeInfo.BaseType == null)
            return false;

        return IsAssignableToGenericType(givenTypeInfo.BaseType, genericType);
    }
    
    private static IServiceCollection RegisterRepositories(IServiceCollection services, IConfiguration configuration,
        params Type[] excludedInterfaces)
    {
        var contextType = typeof(ApplicationDbContext);
        var baseRepositoryType = typeof(EfEntityRepositoryBase<,>);
        var applicationRepositoryType = typeof(ApplicationEntityRepository<>);
        var repositoryTypes = applicationRepositoryType.Assembly
            .GetExportedTypes()
            .Where(type => !(type.IsAbstract || type.IsInterface || type.IsGenericType || type.BaseType == null
                             || type.GetInterfaces().Length == 0
                             || type.Namespace == applicationRepositoryType.Namespace
                             || !type.IsAssignableToGenericType(baseRepositoryType)
                             || !type.BaseType.GetGenericArguments().Any(argumentType => argumentType == contextType)))
            .Select(type => new
            {
                InterfaceType = type.GetInterfaces().Where(i => !i.IsGenericType && !excludedInterfaces.Contains(i))
                    .Reverse().First(),
                ImplementationType = type
            }).ToList();
        
        foreach (var type in repositoryTypes)
        {
            services.AddTransient(type.InterfaceType, type.ImplementationType);
        }

        return services;
        
    }
    
    internal static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        //**********  Base Repositories  **********//
        services
            .AddTransient<IEntityGeneralRepository, ApplicationGeneralRepository>()
            .AddTransient(typeof(IEntityRepository<>), typeof(ApplicationEntityRepository<>));

        //**********  Custom Repositories  **********//
        RegisterRepositories(services, config, typeof(IEntityGeneralRepository), typeof(IEntityRepository<>));

        return services;
    }
    
    internal static IServiceCollection AddDbContextBehavior<TBehavior>(this IServiceCollection services)
        where TBehavior : class, IDbContextBehaivor
    {
        return services.AddScoped<IDbContextBehaivor, TBehavior>();
    }
    
    internal static IServiceCollection AddTransientService(this IServiceCollection services)
    {
        return services
            .AddTransient<ITokenService, TokenService>()
            .AddTransient<IMessageService, MessageService.EmailService>()
            .AddTransient<IMessageService, MessageService.SmsService>()
            .AddTransient<IMessageServiceFactory, MessageServiceFactory>()
            .AddTransient<ITokenService, TokenService>()
            .AddTransient<IDatabaseInitializer, DatabaseInitializer>()
            .AddTransient<ApplicationDbInitializer>();
    }

    internal static IServiceCollection AddSingletonService(this IServiceCollection services)
    {
        return services
            .AddSingleton<IEmailService, EmailService>()
            .AddSingleton<IRedisService, RedisService>()
            .AddSingleton<IMongoDbService, MongoDbService>();
    }
    
    internal static IServiceCollection AutoMapper(this IServiceCollection services)
    {

        var mapperconfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });

        IMapper mapper = mapperconfig.CreateMapper();
        services.AddSingleton(mapper);
        
        
        return services;
    }
}