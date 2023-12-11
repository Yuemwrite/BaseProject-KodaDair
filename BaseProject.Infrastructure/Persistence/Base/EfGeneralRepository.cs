using System.Data;
using System.Data.Common;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Extensions;
using Domain.Base.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Enums;

namespace BaseProject.Infrastructure.Persistence.Base;

public class EfGeneralRepository<TContext> : IEntityGeneralRepository
    where TContext : DbContext
{
    
    protected TContext Context { get; set; }
    public IDbContextBehaivor DbContextBehavior { get; }

    public DatabaseFacade Database => Context.Database;
    
    public EfGeneralRepository(TContext context)
    {
        Context = context;
        DbContextBehavior ??= Context.GetService<IDbContextBehaivor>();
    }
    
    
    
    public IDisposable DisableBehavior(DbContextBehaviorItem behavior)
    {
        return DbContextBehavior.Disable(behavior);
    }

    public IDbContextTransaction BeginTransaction()
    {
        return Database.BeginTransaction();
    }

    public IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel)
    {
        return Database.BeginTransaction(isolationLevel);
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Database.BeginTransactionAsync(cancellationToken: cancellationToken);
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
    {
        return Database.BeginTransactionAsync(isolationLevel, cancellationToken: cancellationToken);
    }

    public void CommitTransaction()
    {
        Database.CommitTransaction();
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Database.CommitTransactionAsync(cancellationToken: cancellationToken);
    }

    public void RollbackTransaction()
    {
        Database.RollbackTransaction();
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Database.RollbackTransactionAsync(cancellationToken: cancellationToken);
    }

    public IQueryable<TEntity> Query<TEntity>() where TEntity : class
    {
        return Context.Set<TEntity>();
    }

    public object Add(object entity)
    {
        return Context.Add(entity).Entity;
    }

    public object Update(object entity)
    {
        Context.Update(entity);
        return entity;
    }

    public object Delete(object entity)
    {
        Context.Remove(entity);
        return entity;
    }

    public int SaveChanges()
    {
        return Context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        
        return await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> Execute(FormattableString interpolatedQueryString, CancellationToken cancellationToken = default)
    {
        return await Context.Database.ExecuteSqlInterpolatedAsync(interpolatedQueryString, cancellationToken: cancellationToken);
    }

    public async Task<List<T>> ExecuteProcedureGetResultAsync<T>(string procedureName, string schemeName = "dbo",
        Dictionary<string, object> procedureParameters = null, CancellationToken cancellationToken = default) where T : class
    {
        var queryBuilder = PrepareSqlFor(procedureName, schemeName: schemeName, procedureParameters: procedureParameters);
        
        return await Context
            .Set<T>()
            .FromSqlRaw(queryBuilder.Query, queryBuilder.Parameters.ToArray())
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<int> ExecuteProcedureAsync(string procedureName, string schemeName = "dbo", Dictionary<string, object> parameters = null,
        CancellationToken cancellationToken = default)
    {
        var queryBuilder = PrepareSqlFor(procedureName, schemeName: schemeName, procedureParameters: parameters);

        return await Context.Database.ExecuteSqlRawAsync(queryBuilder.Query, queryBuilder.Parameters.ToArray(), cancellationToken: cancellationToken);
    }

    public virtual async Task<int> ExecuteRawSqlAsync(string rawSql, string schemeName = "dbo", Dictionary<string, object> parameters = null,
        CancellationToken cancellationToken = default)
    {
        var connectionInfo = await OpenConnection(rawSql, schemeName, parameters, false, cancellationToken);
        var result = await connectionInfo.Command.ExecuteNonQueryAsync(cancellationToken);

        return result;
    }

    public virtual async Task<int> ExecuteProcedureGetIdAsync(string procedureName, string schemeName = "dbo", Dictionary<string, object> parameters = null,
        CancellationToken cancellationToken = default)
    {
        var result = await ExecuteProcedureGetResultAsync(procedureName, schemeName: schemeName, parameters: parameters, cancellationToken: cancellationToken);
        return Convert.ToInt32(result);
    }

    public virtual async Task<object> ExecuteProcedureGetResultAsync(string procedureName, string schemeName = "dbo", Dictionary<string, object> parameters = null,
        CancellationToken cancellationToken = default)
    {
        var connectionInfo = await OpenConnection(procedureName, schemeName, parameters, true, cancellationToken);

        return await connectionInfo.Command.ExecuteScalarAsync(cancellationToken);
    }

    public virtual async Task<DataTable> ExecuteProcedureDataTableAsync(string procedureName, string schemeName = "dbo", Dictionary<string, object> parameters = null,
        CancellationToken cancellationToken = default)
    {
        var connectionInfo = await OpenConnection(string.Empty, string.Empty, parameters, false, cancellationToken);
        var resultTable = new DataTable();
        var tableAdapter = DbProviderFactories.GetFactory(connectionInfo.Connection).CreateDataAdapter();
        var queryBuilder = PrepareSqlFor(procedureName, schemeName: schemeName);

        connectionInfo.Command.CommandType = CommandType.StoredProcedure;
        connectionInfo.Command.CommandText = queryBuilder.Query;
        tableAdapter.SelectCommand = connectionInfo.Command;
        tableAdapter.Fill(resultTable);

        return resultTable;
    }

    public virtual async Task<DataTable> ExecuteRawSqlDataTableAsync(string rawSql, Dictionary<string, object> parameters = null,
        CancellationToken cancellationToken = default)
    {
        var connectionInfo = await OpenConnection(rawSql, null, parameters, false, cancellationToken);
        var resultTable = new DataTable();
        var tableAdapter = DbProviderFactories.GetFactory(connectionInfo.Connection).CreateDataAdapter();

        tableAdapter.SelectCommand = connectionInfo.Command;
        tableAdapter.Fill(resultTable);

        return resultTable;
    }

    public async Task<IPagingExecutionResult<T>> GetPagedResult<T>(IQueryable<T> query, int? pageSize, int? pageIndex, Func<IQueryable<T>, IOrderedQueryable<T>> ordering = null,
        CancellationToken cancellationToken = default)
    {
        if ((pageIndex ??= 1) < 1) pageIndex = 1;
        if ((pageSize ??= 10) < 1) pageSize = 1;

        var hasPaging = false;
        var totalCount = 0;

        if (pageSize.HasValue && pageIndex.HasValue)
        {
            hasPaging = true;
            totalCount = await query.CountAsync(cancellationToken: cancellationToken);
            query = ordering == null ? query : ordering(query);
            query = query.Skip(pageSize.Value * (pageIndex.Value - 1)).Take(pageSize.Value);
        }

        var data = await query.ToListAsync(cancellationToken: cancellationToken);

        return new PagingExecutionResult<T>(data, hasPaging, pageIndex.Value, pageSize.Value, totalCount: totalCount);
    }

    public string GetConnectionString()
    {
        return Context.Database.GetConnectionString()!;
    }
    
    
    protected virtual SqlQueryWrapper PrepareSqlFor(string procedureName, string? schemeName = "dbo",
        Dictionary<string, object> procedureParameters = null)
    {
        return procedureName.ToSqlQueryAndParameters(schemeName, procedureParameters);
    }
    
    private async Task<(DbConnection Connection, DbCommand Command)> OpenConnection(string sql, string schemeName, Dictionary<string, object> parameters, bool asProcedure,
        CancellationToken cancellationToken)
    {
        var connection = Context.Database.GetDbConnection();
        var queryBuilder = PrepareSqlFor(asProcedure ? sql : string.Empty, schemeName: asProcedure ? schemeName : null, parameters);

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = asProcedure ? queryBuilder.Query : sql;
        command.Connection = connection;
        command.Parameters.AddRange(queryBuilder.Parameters.ToArray());

        if (connection.State == ConnectionState.Closed)
            await connection.OpenAsync(cancellationToken);

        return (connection, command);
    }
}

