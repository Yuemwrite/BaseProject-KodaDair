namespace BaseProject.Application.Abstraction.Base;

public interface IPagingExecutionResult<T>
{
    int TotalCount { get; }
    List<T> Data { get; }
    bool HasPaging { get; }
    int CurrentPage { get; }
    int PageSize { get; }
}