using BaseProject.Application.Abstraction.Abstract;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Application.Handlers.Users.Queries;

public class GetCurrentUserProfileQuery : IRequest<Result>
{
    public class GetCurrentUserProfileQueryHandler : IRequestHandler<GetCurrentUserProfileQuery, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEntityRepository<Profile> _profileRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IFollowerRepository _followerRepository;

        public GetCurrentUserProfileQueryHandler(IUserRepository userRepository, IEntityRepository<Profile> profileRepository, ICurrentUser currentUser, IFollowerRepository followerRepository)
        {
            _userRepository = userRepository;
            _profileRepository = profileRepository;
            _currentUser = currentUser;
            _followerRepository = followerRepository;
        }

        public async Task<Result> Handle(GetCurrentUserProfileQuery request, CancellationToken cancellationToken)
        {
            var profile = await
                _profileRepository
                    .Query()
                    .Where(_ => _.UserId == _currentUser.GetUserId())
                    .Include(_ => _.User)
                    .Select(_ => new UserProfileDto()
                    {
                        Id = _.Id,
                        UserId = _.UserId,
                        UserName = _.User.UserName,
                        Title = _.Title,
                        Biography = _.Biography,
                        SocialMediaAddress1  = _.SocialMediaAddress1,
                        SocialMediaAddress2 = _.SocialMediaAddress2,
                        WebSite = _.WebSite,
                        FollowerCount = _followerRepository.Query().Count(f=>f.FollowerUserId == _.UserId),
                        FollowedCount = _followerRepository.Query().Count(f=>f.FollowedUserId == _.UserId),
                        RowVersion = _.RowVersion
                    }).FirstOrDefaultAsync();
            
            
            return await Result<UserProfileDto>
                .SuccessAsync(profile);
        }
    }
}