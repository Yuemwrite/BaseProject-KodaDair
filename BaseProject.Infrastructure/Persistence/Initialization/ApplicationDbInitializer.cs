using System.Diagnostics;
using BaseProject.Infrastructure.Context;
using BaseProject.Infrastructure.Persistence.Initialization.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace BaseProject.Infrastructure.Persistence.Initialization;

public class ApplicationDbInitializer
{
    private readonly ApplicationDbContext _db;

    public ApplicationDbInitializer(ApplicationDbContext db)
    {
        _db = db;
    }

    public void Initialize()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        if (_db.Database.CanConnect())
        {
            
            if (_db.Database.GetMigrations().Any())
            {
               
                _db.Database.Migrate();
                
            }
            
            new UserSeeder(_db).Initialize();
            new CategorySeeder(_db).Initialize();
            new SubCategorySeeder(_db).Initialize();
            
            stopwatch.Stop();
        }
        else
        {
            stopwatch.Stop();
        }
        
        
    }
}