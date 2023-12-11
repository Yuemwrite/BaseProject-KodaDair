using BaseProject.Application.Abstraction.Abstract;
using BaseProject.Infrastructure.Context;
using BaseProject.Infrastructure.Persistence.Base;
using Domain.Concrete;
using Domain.Enum;

namespace BaseProject.Infrastructure.Persistence.Repository;

public class UserRepository :  EfEntityRepositoryBase<User, ApplicationDbContext>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    
}