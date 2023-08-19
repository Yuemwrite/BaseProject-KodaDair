using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Handlers.PageBase;
using BaseProject.Application.Redis;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceStack.Redis;

namespace BaseProject.Application.Handlers.Sharings.Queries;

public class GetFollowersSharingsQuery : IRequest<Result>
{
    public class GetFollowersSharingsQueryHandler : IRequestHandler<GetFollowersSharingsQuery, Result>
    {
        private readonly IEntityRepository<Follower> _followerRepository;
        private readonly IEntityRepository<Sharing> _sharingRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IRedisService _redisService;

        public GetFollowersSharingsQueryHandler(IEntityRepository<Follower> followerRepository,
            IEntityRepository<Sharing> sharingRepository, ICurrentUser currentUser, IRedisService redisService)
        {
            _followerRepository = followerRepository;
            _sharingRepository = sharingRepository;
            _currentUser = currentUser;
            _redisService = redisService;
        }

        public async Task<Result> Handle(GetFollowersSharingsQuery request, CancellationToken cancellationToken)
        {
            var followCount = _followerRepository
                .Query()
                .Count(_ => _.FollowerUserId == _currentUser.GetUserId() && _.IsActive);

            var followers = _redisService.Get<List<Follower>>("followers");

            if (followers is null || followCount != followers.Count)
            {
                followers = await _followerRepository
                    .Query()
                    .AsNoTracking()
                    .Where(_ => _.FollowerUserId == _currentUser.GetUserId() && _.IsActive)
                    .ToListAsync(cancellationToken: cancellationToken);

                _redisService.Add("followers", followers, TimeSpan.FromMinutes(10));
            }


            var sharings = new List<SharingDto>();


            foreach (var follower in followers)
            {
                var followedSharingList = await _sharingRepository
                    .Query()
                    .Where(_ => _.UserId == follower.FollowedUserId && _.IsDeleted == false)
                    .Select(_ => new SharingDto()
                    {
                        Id = _.Id,
                        UserId = follower.FollowedUserId,
                        Title = _.Title,
                        Content = _.Content,
                        CreationTime = _.CreationTime,
                        RowVersion = _.RowVersion,
                        CommentCount = _.Comments.Count(_ => true),
                        LikeCount = _.Likes.Count(_ => true)
                    })
                    .ToListAsync();

                sharings.AddRange(followedSharingList);
            }

            return await Result<List<SharingDto>>.SuccessAsync(sharings);
        }
    }
}