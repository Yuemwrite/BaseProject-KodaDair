using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Wrapper;
using MediatR;

namespace BaseProject.Application.Handlers.PageBase;

public abstract class PagedSearchQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

    protected Result<IEnumerable<T>> HandleResult<T>(IPagingExecutionResult<T> paginationResult)
    {
        return paginationResult.HasPaging
            ? PaginatedResult<T>.Success(paginationResult.Data, paginationResult.TotalCount, paginationResult.CurrentPage, paginationResult.PageSize)
            : Result<IEnumerable<T>>.Success(data: paginationResult.Data);
    }
}