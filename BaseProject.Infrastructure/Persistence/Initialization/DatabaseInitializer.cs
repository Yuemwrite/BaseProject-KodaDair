using Microsoft.Extensions.DependencyInjection;

namespace BaseProject.Infrastructure.Persistence.Initialization;

public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void InitializeDatabases()
    {
        using var scope = _serviceProvider.CreateScope();

        scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>().Initialize();
    }
}