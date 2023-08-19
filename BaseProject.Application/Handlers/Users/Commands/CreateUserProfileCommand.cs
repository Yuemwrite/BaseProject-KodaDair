using BaseProject.Application.Abstraction.Abstract;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using MediatR;
using Shared.Common.Extensions;

namespace BaseProject.Application.Handlers.Users.Commands;

public class CreateUserProfileCommand : IRequest<Result>
{
    public string Title { get; set; }
    public string? Biography { get; set; }
    public string? SocialMediaAddress1 { get; set; }
    public string? SocialMediaAddress2 { get; set; }
    public string? WebSite { get; set; }


    public class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand, Result>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IEntityRepository<Profile> _profileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFollowerRepository _followerRepository;
        
        public CreateUserProfileCommandHandler(ICurrentUser currentUser, IEntityRepository<Profile> profileRepository, IUserRepository userRepository, IFollowerRepository followerRepository)
        {
            _currentUser = currentUser;
            _profileRepository = profileRepository;
            _userRepository = userRepository;
            _followerRepository = followerRepository;
        }

        public async Task<Result> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
        {
            if (_profileRepository.Query().Any(_=>_.UserId == _currentUser.GetUserId()))
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.AlreadyExistProfileInfo).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.AlreadyExistProfileInfo.GetCustomDisplayName() }
                });
            }
            
            
            var profile = new Profile()
            {
                UserId = _currentUser.GetUserId(),
                Biography = request.Biography,
                Title = request.Title,
                SocialMediaAddress1 = request.SocialMediaAddress1,
                SocialMediaAddress2 = request.SocialMediaAddress2,
                WebSite = request.WebSite,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _currentUser.GetUserId()
            };

            _profileRepository.Add(profile);
            await _profileRepository.SaveChangesAsync();
            
            var response = new UserProfileDto()
            {
                Id = profile.Id,
                UserId = profile.UserId,
                UserName = _userRepository.Query().First(u => u.Id == profile.UserId).UserName,
                Biography = profile.Biography,
                Title = profile.Title,
                SocialMediaAddress1 = profile.SocialMediaAddress1,
                SocialMediaAddress2 = profile.SocialMediaAddress2,
                FollowerCount = _followerRepository.Query().Count(f=>f.FollowerUserId == profile.UserId),
                FollowedCount = _followerRepository.Query().Count(f=>f.FollowedUserId == profile.UserId),
                RowVersion = profile.RowVersion
            };
            
            return await Result<UserProfileDto>
                .SuccessAsync(response);
            
        }
    }
}