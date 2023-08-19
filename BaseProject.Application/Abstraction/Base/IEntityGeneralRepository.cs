using System.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Enums;

namespace BaseProject.Application.Abstraction.Base;

public interface IEntityGeneralRepository
{
    DatabaseFacade Database { get; }
    IDbContextBehaivor DbContextBehavior { get; }

    IDisposable DisableBehavior(DbContextBehaviorItem behavior);
    IDbContextTransaction BeginTransaction();
    IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default);
    void CommitTransaction();
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    void RollbackTransaction();
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    IQueryable<TEntity> Query<TEntity>() where TEntity : class;
    object Add(object entity);
    object Update(object entity);
    object Delete(object entity);
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<int> Execute(FormattableString interpolatedQueryString, CancellationToken cancellationToken = default);
    
    Task<List<T>> ExecuteProcedureGetResultAsync<T>(string procedureName, string schemeName = "dbo",
        Dictionary<string, object> procedureParameters = null, CancellationToken cancellationToken = default)
        where T : class;
    Task<int> ExecuteProcedureAsync(string procedureName, string schemeName = "dbo",
        Dictionary<string, object> parameters = null, CancellationToken cancellationToken = default);
    Task<int> ExecuteRawSqlAsync(string rawSql, string schemeName = "dbo",
        Dictionary<string, object> parameters = null, CancellationToken cancellationToken = default);
    Task<int> ExecuteProcedureGetIdAsync(string procedureName, string schemeName = "dbo",
        Dictionary<string, object> parameters = null, CancellationToken cancellationToken = default);
    Task<object> ExecuteProcedureGetResultAsync(string procedureName, string schemeName = "dbo",
        Dictionary<string, object> parameters = null, CancellationToken cancellationToken = default);
    Task<DataTable> ExecuteProcedureDataTableAsync(string procedureName, string schemeName = "dbo",
        Dictionary<string, object> parameters = null, CancellationToken cancellationToken = default);
    Task<DataTable> ExecuteRawSqlDataTableAsync(string rawSql,
        Dictionary<string, object> parameters = null, CancellationToken cancellationToken = default);

    Task<IPagingExecutionResult<T>> GetPagedResult<T>(IQueryable<T> query, int? pageSize = 10, int? pageIndex = 1,
        Func<IQueryable<T>, IOrderedQueryable<T>> ordering = null,
        CancellationToken cancellationToken = default);
    string GetConnectionString();
}