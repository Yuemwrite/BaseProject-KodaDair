using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Handlers.PageBase;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Extensions;

namespace BaseProject.Application.Handlers.Comments.Queries;

public class GetCommentsQuery : PagedSearchQuery<Result>
{
    public Guid SharingId { get; set; }

    public class GetCommentsQueryHandler : PagedSearchQueryHandler<GetCommentsQuery, Result>
    {
        private readonly IEntityRepository<Sharing> _sharingRepository;
        private readonly IEntityRepository<Comment> _commentRepository;

        public GetCommentsQueryHandler(IEntityRepository<Sharing> sharingRepository,
            IEntityRepository<Comment> commentRepository)
        {
            _sharingRepository = sharingRepository;
            _commentRepository = commentRepository;
        }

        public override async Task<Result> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
        {
            if (!_sharingRepository.Query().Any(_ => _.Id == request.SharingId))
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.SharingNotFound).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.SharingNotFound.GetCustomDisplayName() }
                });
            }
            
            var comment = _commentRepository
                .Query()
                .Where(_=>_.SharingId == request.SharingId && _.IsDeleted == false);

            var commentQuery = comment
                .Select(_ => new CommentDto()
                {
                    Id = _.Id,
                    SharingId = _.SharingId,
                    Content = _.Content,
                    UserId = _.UserId,
                    UserName = _.User.UserName,
                    CreationTime = _.CreationTime,
                    ReplyCount = _.Replies.Count(_ => true),
                    LikeCount = _.Likes.Where(_ => _.IsActive).Count(_ => true),
                    RowVersion = _.RowVersion
                }).AsNoTracking();

            var result = await _sharingRepository.GetPagedResult(commentQuery,
                pageSize: request.PageSize,
                pageIndex: request.Page,
                ordering: shr => shr.OrderByDescending(_ => _.CreationTime),
                cancellationToken: cancellationToken);

            return HandleResult(result);
        }
    }
}