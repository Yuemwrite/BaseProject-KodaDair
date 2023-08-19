using BaseProject.Application.Abstraction.Base;
using Domain.Concrete;

namespace BaseProject.Application.Abstraction.Abstract;

public interface IUserRepository : IEntityRepository<User>
{
    
}