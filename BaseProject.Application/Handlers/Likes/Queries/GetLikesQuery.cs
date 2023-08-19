using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Handlers.PageBase;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using MediatR;

namespace BaseProject.Application.Handlers.Likes.Queries;

public class GetLikesQuery : PagedSearchQuery<Result>
{
    public Guid ContentId { get; set; }

    public class GetLikesQueryHandler : PagedSearchQueryHandler<GetLikesQuery, Result>
    {
        private readonly IEntityRepository<Like> _likeRepository;
        private readonly IEntityGeneralRepository _generalRepository;

        public GetLikesQueryHandler(IEntityRepository<Like> likeRepository, IEntityGeneralRepository generalRepository)
        {
            _likeRepository = likeRepository;
            _generalRepository = generalRepository;
        }

        public override async Task<Result> Handle(GetLikesQuery request, CancellationToken cancellationToken)
        {
            var query = _likeRepository.Query();

            if (_generalRepository.Query<Sharing>().Any(_ => _.UID == request.ContentId))
            {
                query = query.Where(_ => _.SharingId == request.ContentId && _.IsDeleted == false);
            }
            
            if (_generalRepository.Query<Comment>().Any(_ => _.UID == request.ContentId))
            {
                query = query.Where(_ => _.CommentId == request.ContentId && _.IsDeleted == false);
            }

            if (_generalRepository.Query<Reply>().Any(_ => _.UID == request.ContentId))
            {
                query = query.Where(_ => _.ReplyId == request.ContentId && _.IsDeleted == false);
            }
            

            var likeQuery = query
                .Select(_ => new LikeDto()
                {
                    Id = _.Id,
                    SharingId = _.SharingId,
                    CommentId = _.CommentId,
                    ReplyId = _.ReplyId,
                    IsActive = _.IsActive,
                    UserId = _.UserId,
                    UserName = _generalRepository.Query<User>().First(u => u.Id == _.UserId).UserName,
                    RowVersion = _.RowVersion
                });

            var result = await _likeRepository.GetPagedResult(likeQuery,
                pageSize: request.PageSize,
                pageIndex: request.Page,
                ordering: shr => shr.OrderByDescending(_ => _.Id),
                cancellationToken: cancellationToken);

            return HandleResult(result);
        }
    }
}