using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Handlers.PageBase;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Application.Handlers.Users.Queries;

public class GetPendingFollowRequestsQuery : PagedSearchQuery<Result>
{
    public class GetPendingFollowRequestsQueryHandler : PagedSearchQueryHandler<GetPendingFollowRequestsQuery, Result>
    {
        private readonly IEntityRepository<Follower> _followerRepository;
        private readonly ICurrentUser _currentUser;

        public GetPendingFollowRequestsQueryHandler(IEntityRepository<Follower> followerRepository,
            ICurrentUser currentUser)
        {
            _followerRepository = followerRepository;
            _currentUser = currentUser;
        }

        public override async Task<Result> Handle(GetPendingFollowRequestsQuery request,
            CancellationToken cancellationToken)
        {
            var pendingFollowerRequests =
                _followerRepository
                    .Query()
                    .Where(_ => _.FollowedUserId == _currentUser.GetUserId() &&
                                _.ApprovalStatus == ApprovalStatus.Pending);
            

            var pendingFollowerRequestsQuery = pendingFollowerRequests
                .Include(_ => _.FollowerUser)
                .Select(_ => new PendingFollowerRequestDto()
                {
                    Id = _.Id,
                    UserId = _.FollowerUserId,
                    UserName = _.FollowerUser.UserName,
                    CreationTime = _.CreationTime,
                    RowVersion = _.RowVersion
                }).AsNoTracking();

            var result = await _followerRepository.GetPagedResult(pendingFollowerRequestsQuery,
                pageSize: request.PageSize,
                pageIndex: request.Page,
                ordering: shr => shr.OrderByDescending(_ => _.CreationTime),
                cancellationToken: cancellationToken);
            
            return HandleResult(result);
        }
    }
}