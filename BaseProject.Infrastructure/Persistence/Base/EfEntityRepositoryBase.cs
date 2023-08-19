using System.Linq.Expressions;
using BaseProject.Application.Abstraction.Base;
using Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Infrastructure.Persistence.Base;

public class EfEntityRepositoryBase<TEntity, TContext> : EfGeneralRepository<TContext>, IEntityRepository<TEntity>
    where TEntity : class, IEntity
    where TContext : DbContext
{
    public EfEntityRepositoryBase(TContext context) : base(context)
    {
    }

    public TEntity Add(TEntity entity)
    {
        return Context.Add(entity).Entity;
    }

    public TEntity Update(TEntity entity)
    {
        Context.Update(entity);
        return entity;
    }

    public TEntity Delete(TEntity entity)
    {
        Context.Remove(entity);
        return entity;
    }

    public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> expression = null)
    {
        return expression == null
            ? Context.Set<TEntity>().AsNoTracking()
            : Context.Set<TEntity>().Where(expression).AsNoTracking();
    }

    public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression = null, bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        return expression == null
            ? await Context.Set<TEntity>().ToListAsync(cancellationToken: cancellationToken)
            : await Context.Set<TEntity>().Where(expression).ToListAsync(cancellationToken: cancellationToken);
    }

    public TEntity Get(Expression<Func<TEntity, bool>> expression)
    {
        return Context.Set<TEntity>().FirstOrDefault(expression)!;
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        var result = await Context.Set<TEntity>().AsQueryable().FirstOrDefaultAsync(expression, cancellationToken: cancellationToken);

        return result!;
    }

    public IQueryable<TEntity> Query()
    {
        return Context.Set<TEntity>();
    }
}