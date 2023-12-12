using BaseProject.Infrastructure.Context;
using BenchmarkDotNet.Attributes;
using Domain.Concrete;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.ConsoleApplication;

[MemoryDiagnoser]
public class BenchMarkService
{
    private ApplicationDbContext _context;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("")
            .Options;

        _context = new ApplicationDbContext(dbContextOptions);
    }

    [Benchmark]
    public List<User> GetUser()
    {
        
        var users = _context.Users
            .ToList();

        return users;
    }
    
    [Benchmark]
    public async Task<List<User>>  GetUserAsync()
    {
        
        var users = await _context.Users
            .ToListAsync(); 

        return users;
    }
}