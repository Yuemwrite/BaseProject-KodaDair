namespace BaseProject.Application.Wrapper;

public class PaginatedResult<T> : Result<IEnumerable<T>>
{
    public int CurrentPage { get; set; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;

    public PaginatedResult(IEnumerable<T> data)
    {
        Data = data;
    }
    
    protected PaginatedResult(
        bool succeeded,
        IEnumerable<T>? data = default,
        List<string>? messages = null,
        int totalCount = 0,
        int page = 1,
        int pageSize = 10)
    {
        Data = data;
        Messages = messages;
        Succeeded = succeeded;
        CurrentPage = page < 1 ? 1 : page;
        PageSize = pageSize < 5 ? 5 : pageSize;
        TotalCount = totalCount < 0 ? 0 : totalCount;
    }
    
    public static PaginatedResult<T> Failure(List<string> messages)
    {
        return new(false, messages: messages);
    }

    public static PaginatedResult<T> Success(IEnumerable<T> data, int totalCount, int page, int pageSize)
    {
        return new(true, data: data, totalCount: totalCount, page: page, pageSize: pageSize);
    }
}