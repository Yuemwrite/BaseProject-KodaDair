using System.Linq.Expressions;
using Domain.Base;

namespace BaseProject.Application.Abstraction.Base;

public interface IEntityRepository<T> :  IEntityGeneralRepository where T : class, IEntity
{
    T Add(T entity);
    T Update(T entity);
    T Delete(T entity);
    IEnumerable<T> GetList(Expression<Func<T, bool>> expression = null);
    Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> expression = null, bool asNoTracking = true, CancellationToken cancellationToken = default);
    T Get(Expression<Func<T, bool>> expression);
    Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    IQueryable<T> Query();
}