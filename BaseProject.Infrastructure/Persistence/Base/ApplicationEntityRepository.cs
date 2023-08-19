using BaseProject.Infrastructure.Context;
using Domain.Base;

namespace BaseProject.Infrastructure.Persistence.Base;

public class ApplicationEntityRepository<TEntity> : EfEntityRepositoryBase<TEntity, ApplicationDbContext>
    where TEntity : class, IEntity
{
    public ApplicationEntityRepository(ApplicationDbContext context) : base(context)
    {
    }
}