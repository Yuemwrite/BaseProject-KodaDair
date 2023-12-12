using BaseProject.Application.Abstraction.Base;
using BenchmarkDotNet.Attributes;
using Domain.Concrete;

namespace BaseProject.Application.Abstraction.Abstract;

public interface IUserRepository : IEntityRepository<User>
{
    public Task<List<User>> GetUsers();
    public Task<List<User>> GetAsNoTrackingUsers();

    [Benchmark]
    public Task BenchMarkTest();
}