using BaseProject.Application.Abstraction.Abstract;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Extensions;

namespace BaseProject.Application.Handlers.Users.Commands;

public class UpdateUserProfileCommand : IRequest<Result>
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    public string? Biography { get; set; }
    public string? SocialMediaAddress1 { get; set; }
    public string? SocialMediaAddress2 { get; set; }
    public string? WebSite { get; set; }
    
    public byte[] RowVersion { get; set; }
    
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, Result>
    {
        private readonly IEntityRepository<Profile> _profileRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUserRepository _userRepository;
        private readonly IFollowerRepository _followerRepository;

        public UpdateUserProfileCommandHandler(IEntityRepository<Profile> profileRepository, ICurrentUser currentUser, IUserRepository userRepository, IFollowerRepository followerRepository)
        {
            _profileRepository = profileRepository;
            _currentUser = currentUser;
            _userRepository = userRepository;
            _followerRepository = followerRepository;
        }

        public async Task<Result> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await
                _profileRepository
                    .Query()
                    .FirstOrDefaultAsync(_ => _.Id == request.Id && _.UserId == _currentUser.GetUserId());

            if (profile is null)
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.ProfileInfoNotFound).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.ProfileInfoNotFound.GetCustomDisplayName() }
                });
            }

            profile.Biography = request.Biography;
            profile.Title = request.Title;
            profile.SocialMediaAddress1 = request.SocialMediaAddress1;
            profile.SocialMediaAddress2 = request.SocialMediaAddress2;
            profile.WebSite = request.WebSite;
            profile.RowVersion = request.RowVersion;

            _profileRepository.Update(profile);
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