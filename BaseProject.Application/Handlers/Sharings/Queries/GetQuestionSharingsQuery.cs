using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Handlers.PageBase;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Application.Handlers.Sharings.Queries;

public class GetQuestionSharingsQuery : PagedSearchQuery<Result>
{
    public Guid? UserId { get; set; }

    public class GetQuestionSharingsQueryHandler : PagedSearchQueryHandler<GetQuestionSharingsQuery, Result>
    {
        private readonly IEntityRepository<Sharing> _sharingRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IEntityRepository<Follower> _followerRepository;

        public GetQuestionSharingsQueryHandler(IEntityRepository<Sharing> sharingRepository, ICurrentUser currentUser,
            IEntityRepository<Follower> followerRepository)
        {
            _sharingRepository = sharingRepository;
            _currentUser = currentUser;
            _followerRepository = followerRepository;
        }

        public override async Task<Result> Handle(GetQuestionSharingsQuery request, CancellationToken cancellationToken)
        {
            var sharing = _sharingRepository
                .Query();

            sharing = request.UserId.HasValue ? sharing.Where(_ => _.UserId == request.UserId) : sharing;


            var sharingQuery = sharing
                .Include(_ => _.Comments)
                .Include(_ => _.Likes)
                .Include(_ => _.User)
                .Where(_=>_.IsDeleted == false)
                .Select(_ => new SharingDto()
                {
                    Id = _.Id,
                    UserId = _.UserId,
                    UserName = _.User.UserName,
                    Title = _.Title,
                    Content = _.Content,
                    IsFixed = _.IsFixed,
                    CreationTime = _.CreationTime,
                    CommentCount = _.Comments.Count(_ => true),
                    LikeCount = _.Likes.Where(_ => _.IsActive).Count(_ => true),
                    RowVersion = _.RowVersion
                }).AsNoTracking();


            var result = await _sharingRepository.GetPagedResult(sharingQuery,
                pageSize: request.PageSize,
                pageIndex: request.Page,
                ordering: shr => shr.OrderByDescending(_ => _.Id),
                cancellationToken: cancellationToken);


            if (result.Data.Any(_ => _.IsFixed))
            {
                var fixedData = result.Data.First(_ => _.IsFixed);

                var index = result.Data.IndexOf(fixedData);

                if (index == -1 || index == 0) return HandleResult(result);
                result.Data.Remove(fixedData);
                result.Data.Insert(0, fixedData);
            }


            return HandleResult(result);
        }
    }
}