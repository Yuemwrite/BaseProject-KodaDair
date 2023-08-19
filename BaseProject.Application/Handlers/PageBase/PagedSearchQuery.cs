using MediatR;

namespace BaseProject.Application.Handlers.PageBase;

public abstract class PagedSearchQuery<T, TPrimaryKey> : IRequest<T>
{
    public TPrimaryKey Id { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}