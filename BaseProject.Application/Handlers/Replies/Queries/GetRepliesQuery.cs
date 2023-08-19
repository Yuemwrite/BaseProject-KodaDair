using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Handlers.PageBase;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Extensions;

namespace BaseProject.Application.Handlers.Replies.Queries;

public class GetRepliesQuery : PagedSearchQuery<Result>
{
    public Guid CommentId { get; set; }

    public class GetRepliesQueryHandler : PagedSearchQueryHandler<GetRepliesQuery, Result>
    {
        private readonly IEntityRepository<Comment> _commentRepository;
        private readonly IEntityRepository<Reply> _replyRepository;

        public GetRepliesQueryHandler(IEntityRepository<Comment> commentRepository,
            IEntityRepository<Reply> replyRepository)
        {
            _commentRepository = commentRepository;
            _replyRepository = replyRepository;
        }

        public override async Task<Result> Handle(GetRepliesQuery request, CancellationToken cancellationToken)
        {

            if (!_commentRepository.Query().Any(_=>_.Id == request.CommentId))
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.CommentNotFound).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.CommentNotFound.GetCustomDisplayName() }
                });
            }

            var reply = _replyRepository
                .Query()
                .Include(_ => _.Comment)
                .Include(_ => _.Likes)
                .Include(_ => _.User)
                .Where(_ => _.CommentId == request.CommentId);

            var replyQuery = reply
                .Select(_ => new ReplyDto()
                {
                    Id = _.Id,
                    CommentId = _.CommentId,
                    UserId = _.UserId,
                    UserName = _.User.UserName,
                    Content = _.Content,
                    CreationTime = _.CreationTime,
                    LikeCount = _.Likes.Where(_=>_.IsActive).Count(like => true),
                    RowVersion = _.RowVersion
                }).AsNoTracking();

            var result = await _replyRepository.GetPagedResult(replyQuery,
                pageSize: request.PageSize,
                pageIndex: request.Page,
                ordering: rply => rply.OrderByDescending(_ => _.CreationTime),
                cancellationToken: cancellationToken);

            return HandleResult(result);
        }
    }
}