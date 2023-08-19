using BaseProject.Application.Abstraction.Abstract;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Extensions;

namespace BaseProject.Application.Handlers.Users.Commands;

public class CreateFollowerCommand : IRequest<Result>
{
    public Guid FollowedUserId { get; set; }

    public class CreateFollowerCommandHandler : IRequestHandler<CreateFollowerCommand, Result>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IFollowerRepository _followerRepository;
        private readonly IUserRepository _userRepository;


        public CreateFollowerCommandHandler(ICurrentUser currentUser,
            IUserRepository userRepository, IFollowerRepository followerRepository)
        {
            _currentUser = currentUser;
            _userRepository = userRepository;
            _followerRepository = followerRepository;
        }

        public async Task<Result> Handle(CreateFollowerCommand request, CancellationToken cancellationToken)
        {
            var followedUser = await
                _userRepository
                    .Query()
                    .FirstOrDefaultAsync(_ => _.Id == request.FollowedUserId, cancellationToken: cancellationToken);

            if (followedUser is null)
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.UserNotFound).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.UserNotFound.GetCustomDisplayName() }
                });
            }

            var followerInfo = await
                _followerRepository
                    .Query()
                    .FirstOrDefaultAsync(_ =>
                            _.FollowerUserId == _currentUser.GetUserId() && _.FollowedUserId == request.FollowedUserId,
                        cancellationToken: cancellationToken);

            var response = new FollowerDto();


            if (followerInfo is null)
            {
                var follow = new Follower
                {
                    FollowerUserId = _currentUser.GetUserId(),
                    FollowedUserId = request.FollowedUserId,
                    CreatorUserId = _currentUser.GetUserId(),
                    CreationTime = DateTime.UtcNow,
                    IsActive = !followedUser.IsPrivate,
                    ApprovalStatus = followedUser.IsPrivate ? ApprovalStatus.Pending : ApprovalStatus.Approved
                };

                _followerRepository.Add(follow);
                await _followerRepository.SaveChangesAsync();

                response.FollowerUserId = follow.FollowerUserId;
                response.UserId = follow.FollowedUserId;
                response.UserName = _userRepository.Query().FirstOrDefault(_ => _.Id == follow.FollowedUserId)!
                    .UserName;
                response.ApprovalStatus = follow.ApprovalStatus;
                response.FollowerUserName =
                    _userRepository.Query().FirstOrDefault(_ => _.Id == follow.FollowerUserId)!.UserName;
                response.CreationTime = follow.CreationTime;
                response.RowVersion = follow.RowVersion;


                return await Result<FollowerDto>
                    .SuccessAsync(response);
            }

            followerInfo.IsActive = !followedUser.IsPrivate;
            followerInfo.ApprovalStatus =
                _followerRepository.ChangeApprovalStatus(followerInfo.IsActive, followedUser.IsPrivate);

            _followerRepository.Update(followerInfo);
            await _followerRepository.SaveChangesAsync();

            response.FollowerUserId = followerInfo.FollowerUserId;
            response.UserId = followerInfo.FollowedUserId;
            response.UserName = _userRepository.Query().FirstOrDefault(_ => _.Id == followerInfo.FollowedUserId)!
                .UserName;
            response.ApprovalStatus = followerInfo.ApprovalStatus;
            response.FollowerUserName =
                _userRepository.Query().FirstOrDefault(_ => _.Id == followerInfo.FollowerUserId)!.UserName;
            response.RowVersion = followerInfo.RowVersion;

            return await Result<FollowerDto>
                .SuccessAsync(response);
        }
    }
}