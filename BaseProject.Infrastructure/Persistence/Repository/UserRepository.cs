using BaseProject.Application.Abstraction.Abstract;
using BaseProject.Infrastructure.Context;
using BaseProject.Infrastructure.Persistence.Base;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Domain.Concrete;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Infrastructure.Persistence.Repository;

public class UserRepository :  EfEntityRepositoryBase<User, ApplicationDbContext>, IUserRepository
{
    
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }


    [Benchmark]
    public async Task<List<User>> GetUsers()
    {
        var users = await Query()
            .ToListAsync();

        return users;
    }

    [Benchmark]
    public async Task<List<User>> GetAsNoTrackingUsers()
    {
        var users = await Query()
            .AsNoTracking()
            .ToListAsync();

        return users;
    }

    public async Task BenchMarkTest()
    {
       var result = BenchmarkRunner.Run<UserRepository>();
       Console.WriteLine(result);
    }
}