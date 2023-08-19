using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BaseProject.Infrastructure.Context;

public static class Startup
{
    public static IServiceCollection RegisterDatabase(this IServiceCollection services, IConfiguration config)
    {
        return services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("SqlServer"), sqlOptions =>
            {
                sqlOptions.MigrationsAssembly("Migratiors.Local");
            });
            options.UseLazyLoadingProxies();
        });
            
    }
}